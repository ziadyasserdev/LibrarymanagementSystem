using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Commands.RestoreAuthor
{
    public class RestoreAuthorCommandHandler : IRequestHandler<RestoreAuthorCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public RestoreAuthorCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(RestoreAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await unitOfWork.Authors.Query()
                .FirstOrDefaultAsync(x => x.AuthorId == request.Id);
            if (author == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Author not found");
            if(!author.IsDeleted)
                return Result<int>.Failure(ResultStatus.Failure, "Author already not deleted");
            var checkDuplicate = await unitOfWork.Authors.Query()
                .AnyAsync(x => x.Name == author.Name && !x.IsDeleted && x.AuthorId != author.AuthorId);
            if (checkDuplicate)
                return Result<int>.Failure(ResultStatus.Failure, "Another active author with the same name already exists. Please rename the author before restoring it");
            author.IsDeleted = false;
            author.DeletedAt = null;
            author.UpdatedAt = DateTime.Now;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(author.AuthorId);
        }
    }
}
