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

namespace LibrarymanagementSystem.Application.Features.BookCopies.Commands.BulkDeleteBookCopies
{
    public class BulkDeleteBookCopiesCommandHandler : IRequestHandler<BulkDeleteBookCopiesCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public BulkDeleteBookCopiesCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(BulkDeleteBookCopiesCommand request, CancellationToken cancellationToken)
        {
            var bookCopies = await unitOfWork.BookCopies
               .Query()
               .Where(x => request.Ids.Contains(x.Id))
               .ToListAsync(cancellationToken);

            if (!bookCopies.Any())
                return Result<string>.Failure(ResultStatus.NotFound,
                    "No book copies found");

          
            var invalidCopies = bookCopies
                .Where(x => x.IsDeleted
                    || x.IsLost
                    || x.Status == BookCopyStatus.Loaned)
                .ToList();

            if (invalidCopies.Any())
                return Result<string>.Failure(ResultStatus.Conflict,
                    "Some book copies cannot be deleted");

           

            foreach (var copy in bookCopies)
            {
                copy.IsDeleted = true;
                copy.DeletedAt = DateTime.Now;
                copy.UpdatedAt = DateTime.Now;
            }

            await unitOfWork.SaveAsync();

            return Result<string>.Success(
                $"{bookCopies.Count} Book copies deleted successfully");

        }
    }
}
