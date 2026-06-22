using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Commands.AddBookCopy
{
    public class AddBookCopyCommandHandler : IRequestHandler<AddBookCopyCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public AddBookCopyCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(AddBookCopyCommand request, CancellationToken cancellationToken)
        {
            var book = await unitOfWork.Books.Query()
                .FirstOrDefaultAsync(x => x.Id == request.BookId && x.IsActive && !x.IsDeleted, cancellationToken);
            if (book == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Book not found or inactive/deleted");
            var checkDuplicateBarcode = await unitOfWork.BookCopies.Query()
                .AnyAsync(x => x.Barcode == request.Barcode && !x.IsDeleted, cancellationToken);
            if (checkDuplicateBarcode)
                return Result<int>.Failure(ResultStatus.Conflict, "A book copy with the same barcode already exists");
            var location = await unitOfWork.Locations.Query()
                .FirstOrDefaultAsync(x => x.Id == request.LocationId && x.IsActive && !x.IsDeleted, cancellationToken);
            if (location == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Location not found or inactive/deleted");
            var usedLocation = await unitOfWork.BookCopies.Query()
                .CountAsync(x => x.LocationId == request.LocationId && !x.IsDeleted, cancellationToken);
            if(usedLocation >= location.Capacity)
                return Result<int>.Failure(ResultStatus.Conflict, "Location capacity exceeded. Cannot add more book copies to this location.");
            var bookCopy = new BookCopy
            {
                BookId = request.BookId,
                Barcode = request.Barcode,
                LocationId= request.LocationId,
                CreatedAt = DateTime.UtcNow,
            };

            await unitOfWork.BookCopies.AddAsync(bookCopy);
            await unitOfWork.SaveAsync();
            return Result<int>.Success(bookCopy.Id);
        }
    }
}
