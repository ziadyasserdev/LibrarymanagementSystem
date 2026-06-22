using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.RestoreBook
{
    public class RestoreBookCommandHandler : IRequestHandler<RestoreBookCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public RestoreBookCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(RestoreBookCommand request, CancellationToken cancellationToken)
        {
            var book = await unitOfWork.Books.Query()
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            if(book == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Book not found");
            if(!book.IsDeleted)
                return Result<int>.Failure(ResultStatus.Conflict, "Book is not deleted to restore it");
            var checkDup=await unitOfWork.Books.Query()
                .AnyAsync(x => x.Title == book.Title
                && !x.IsDeleted
                && x.Id != book.Id
                && x.AuthorId == book.AuthorId);
            if (checkDup)
                return Result<int>.Failure(ResultStatus.Conflict, "A book with the same title and author already exists. Cannot restore this book.");
            book.IsDeleted= false;
            book.DeletedAt = null;
            book.UpdatedAt = DateTime.UtcNow;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(book.Id);
        }
    }
}
