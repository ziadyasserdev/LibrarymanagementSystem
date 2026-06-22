using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.DeactivateBook
{
    public class DeactivateBookCommandHandler : IRequestHandler<DeactivateBookCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeactivateBookCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(DeactivateBookCommand request, CancellationToken cancellationToken)
        {
            var book = await unitOfWork.Books.Query()
                .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted);
            if (book is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Book not found or deleted");
            if(!book.IsActive)
                return Result<int>.Failure(ResultStatus.Conflict, "Book is already deactivated");
            var hasCopies=await unitOfWork.BookCopies.Query()
                .AnyAsync(x => x.BookId == request.Id && !x.IsDeleted, cancellationToken);
            if(hasCopies)
                return Result<int>.Failure(ResultStatus.Conflict, "Cannot deactivate book with existing copies. Please remove all copies first.");
            book.IsActive = false;
            book.UpdatedAt = DateTime.Now;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(book.Id);

        }
    }
}
