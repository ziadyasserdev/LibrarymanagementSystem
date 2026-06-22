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

namespace LibrarymanagementSystem.Application.Features.BookCopies.Commands.DeleteBookCopy
{
    public class DeleteBookCopyCommandHandler : IRequestHandler<DeleteBookCopyCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeleteBookCopyCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(DeleteBookCopyCommand request, CancellationToken cancellationToken)
        {
           

            var bookCopy = await unitOfWork.BookCopies.Query()
                .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

            if (bookCopy == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Book copy not found or already deleted.");

            if (bookCopy.Status == BookCopyStatus.Loaned)
                return Result<int>.Failure(ResultStatus.Conflict, "Cannot delete a book copy that is currently loaned out");

            if(bookCopy.IsLost)
                return Result<int>.Failure(ResultStatus.Conflict, "Cannot delete a book copy that is marked as lost");

            bookCopy.IsDeleted = true;
            bookCopy.DeletedAt = DateTime.Now;
            bookCopy.UpdatedAt = DateTime.Now;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(bookCopy.Id, $"Book copy '{bookCopy.Barcode}' deleted successfully");
        }
    }
}
