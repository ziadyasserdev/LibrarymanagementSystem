using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.CanDeleteBook
{
    public class CanDeleteBookQueryHandler : IRequestHandler<CanDeleteBookQuery, Result<bool>>
    {
        private readonly IUnitOfWork unitOfWork;

        public CanDeleteBookQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<bool>> Handle(CanDeleteBookQuery request, CancellationToken cancellationToken)
        {
            var book = await unitOfWork.Books.Query()
                .FirstOrDefaultAsync(x => x.Id == request.BookId);
            if(book is null)
                return Result<bool>.Failure(ResultStatus.NotFound, "Book not found");
            if(book.IsDeleted)
                return Result<bool>.Failure(ResultStatus.Conflict, "Book is already deleted");
            var hasCopies=await unitOfWork.BookCopies.Query()
                .AnyAsync(x => x.BookId == request.BookId && !x.IsDeleted, cancellationToken);
            if(hasCopies)
                return Result<bool>.Failure(ResultStatus.Conflict, "Cannot delete book with existing copies");
            return Result<bool>.Success(true);
        }
    }
}
