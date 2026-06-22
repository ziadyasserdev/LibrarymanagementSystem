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

namespace LibrarymanagementSystem.Application.Features.BookCopies.Commands.EditBookCopy
{
    public class EditBookCopyCommandHandler : IRequestHandler<EditBookCopyCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public EditBookCopyCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(EditBookCopyCommand request, CancellationToken cancellationToken)
        {
           var bookCopy=await unitOfWork.BookCopies.Query()
                .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);
            if (bookCopy == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Book copy not found or deleted");

            if(bookCopy.IsLost)
                return Result<int>.Failure(ResultStatus.Conflict, "Cannot edit a lost book copy. Please mark it as found first.");

            if(bookCopy.Status == BookCopyStatus.Loaned)
                return Result<int>.Failure(ResultStatus.Conflict, "Cannot edit a loaned book copy. Please wait until it is returned.");


            var checkDuplicate=await unitOfWork.BookCopies.Query()
                .AnyAsync(x => x.Barcode == request.Barcode
                && !x.IsDeleted
                && x.Id != request.Id, cancellationToken);
            if(checkDuplicate)
                return Result<int>.Failure(ResultStatus.Conflict, "A book copy with the same barcode already exists");
            var bookExists = await unitOfWork.Books.Query()
                .AnyAsync(x => x.Id == request.BookId
                    && !x.IsDeleted
                    && x.IsActive,
                    cancellationToken);

            if (!bookExists)
                return Result<int>.Failure(ResultStatus.NotFound,
                    "Book not found or inactive/deleted");
            var location=await unitOfWork.Locations.Query()
                .FirstOrDefaultAsync(x => x.Id == request.LocationId 
                && !x.IsDeleted && x.IsActive, cancellationToken);

            if(location is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Location not found or inactive/deleted");
           

            if (bookCopy.LocationId != request.LocationId)
            {
                var usedLocation = await unitOfWork.BookCopies.Query()
                    .CountAsync(x => x.LocationId == location.Id
                    && !x.IsDeleted, cancellationToken);

                if (usedLocation >= location.Capacity)
                    return Result<int>.Failure(ResultStatus.Conflict,
                        "Location capacity exceeded. Cannot move book copy to this location.");
            }
            bookCopy.Barcode = request.Barcode;
            bookCopy.BookId = request.BookId;
            bookCopy.LocationId=request.LocationId;
            bookCopy.UpdatedAt = DateTime.UtcNow;
            await unitOfWork.SaveAsync();

            return Result<int>.Success(bookCopy.Id);
        }
    }
}
