using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Authors.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.CanDeleteAuthor
{
    public class CanDeleteAuthorQueryHandler : IRequestHandler<CanDeleteAuthorQuery, Result<CanDeleteAuthorDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public CanDeleteAuthorQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<CanDeleteAuthorDto>> Handle(CanDeleteAuthorQuery request, CancellationToken cancellationToken)
        {
            var author = await unitOfWork.Authors.Query()
    .FirstOrDefaultAsync(x => x.AuthorId == request.AuthorId, cancellationToken);

            if (author == null)
            {
                return Result<CanDeleteAuthorDto>.Success(new CanDeleteAuthorDto
                {
                    AuthorId = request.AuthorId,
                    CanDelete = false,
                    Reason = "Author not found"
                });
            }

            if (author.IsDeleted)
            {
                return Result<CanDeleteAuthorDto>.Success(new CanDeleteAuthorDto
                {
                    AuthorId = request.AuthorId,
                    CanDelete = false,
                    Reason = "Author is already deleted"
                });
            }

            var hasBooks = await unitOfWork.Books.Query()
                .AnyAsync(b => b.AuthorId == request.AuthorId && !b.IsDeleted, cancellationToken);

            if (hasBooks)
            {
                return Result<CanDeleteAuthorDto>.Success(new CanDeleteAuthorDto
                {
                    AuthorId = request.AuthorId,
                    CanDelete = false,
                    Reason = "Author has active books"
                });
            }

         
            return Result<CanDeleteAuthorDto>.Success(new CanDeleteAuthorDto
            {
                AuthorId = author.AuthorId,
                CanDelete = true,
                Reason = null
            });
        }
    }
}
