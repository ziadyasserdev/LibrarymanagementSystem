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

namespace LibrarymanagementSystem.Application.Features.Authors.Commands.ActiveAuthor
{
    public class ActiveAuthorCommandHandler : IRequestHandler<ActiveAuthorCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public ActiveAuthorCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(ActiveAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await unitOfWork.Authors.Query()
           .FirstOrDefaultAsync(x => x.AuthorId == request.Id && !x.IsDeleted);
            if (author == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Author not found");

            if (author.IsActive)
                return Result<int>.Failure(ResultStatus.Failure, "Author is already active");
            author.IsActive = true;
            author.UpdatedAt = DateTime.Now;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(author.AuthorId);
        }
    }
}
