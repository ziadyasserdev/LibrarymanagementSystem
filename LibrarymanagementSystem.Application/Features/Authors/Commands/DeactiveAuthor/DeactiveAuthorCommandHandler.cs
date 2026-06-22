using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Commands.DeactiveAuthor
{
    public class DeactiveAuthorCommandHandler : IRequestHandler<DeactiveAuthorCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeactiveAuthorCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(DeactiveAuthorCommand request, CancellationToken cancellationToken)
        {
           var author=await unitOfWork.Authors.Query()
                .FirstOrDefaultAsync(x => x.AuthorId == request.Id 
                && !x.IsDeleted);
            if (author == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Author not found");
            if(!author.IsActive)
                return Result<int>.Failure(ResultStatus.Failure, "Author is already deactive");
            author.IsActive = false;
            author.UpdatedAt = DateTime.Now;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(author.AuthorId);
        }
    }
}
