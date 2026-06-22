using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Commands.DeleteAuthor
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeleteAuthorCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await unitOfWork.Authors.Query()
                .FirstOrDefaultAsync(x => x.AuthorId == request.Id && !x.IsDeleted);
            if(author == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Author not found or deleted");
            var hasBooks = await unitOfWork.Books.Query()
    .AnyAsync(b => b.AuthorId == author.AuthorId && !b.IsDeleted, cancellationToken);

            if (hasBooks)
                return Result<int>.Failure(ResultStatus.Conflict,
                    "Cannot delete author with active books.");
            author.IsDeleted=true;
            author.IsActive = false;
            author.DeletedAt = DateTime.Now;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(author.AuthorId);
        }
    }
}
