using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Commands.BulkRestoreBookCopies
{
    public class BulkRestoreBookCopiesCommandHandler : IRequestHandler<BulkRestoreBookCopiesCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public BulkRestoreBookCopiesCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(BulkRestoreBookCopiesCommand request, CancellationToken cancellationToken)
        {
            var Ids= request.Ids.Distinct().ToList();
            var bookCopies = await unitOfWork.BookCopies
             .Query()
             .Where(x => Ids.Contains(x.Id))
             .ToListAsync(cancellationToken);


            if (!bookCopies.Any())
                return Result<string>.Failure(ResultStatus.NotFound,
                    "No book copies found");
            var invalidCopies = bookCopies
               .Where(x => !x.IsDeleted
                   || x.IsLost
                   || x.Status == BookCopyStatus.Loaned)
               .ToList();

            if (invalidCopies.Any())
                return Result<string>.Failure(ResultStatus.Conflict,
                    "Some book copies cannot be restored");

           

            foreach(var copy in bookCopies)
            {
                var duplicateBarcode = await unitOfWork.BookCopies.Query()
                    .AnyAsync(x => x.Barcode == copy.Barcode 
                    && !x.IsDeleted
                    && x.Id != copy.Id
                    , cancellationToken);
                if (duplicateBarcode)
                    return Result<string>.Failure(ResultStatus.Conflict,
                        $"A book copy with barcode {copy.Barcode} already exists");
                copy.IsDeleted = false;
                copy.DeletedAt = null;
                copy.UpdatedAt = DateTime.Now;
            }
            await unitOfWork.SaveAsync();
            return Result<string>.Success($"{bookCopies.Count} Book copies restored successfully");
        }
    }
}
