using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.DeleteBook
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeleteBookCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var book = await unitOfWork.Books.Query()
                .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);
            if (book == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Book not found or deleted");
           var hasCopies=await unitOfWork.BookCopies.Query()
                .AnyAsync(x => x.BookId == request.Id && !x.IsDeleted);
            if(hasCopies)
                return Result<int>.Failure(ResultStatus.Conflict, "Book cannot be deleted while active copies exist in the system");
            book.IsDeleted = true;
            book.DeletedAt = DateTime.Now;
            
            book.IsActive = false;
           
            await unitOfWork.SaveAsync();
            return Result<int>.Success(book.Id);
        }
    }
}
