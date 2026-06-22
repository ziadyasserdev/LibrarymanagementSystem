using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateAuthorCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await unitOfWork.Authors.Query()
                .FirstOrDefaultAsync(a => a.AuthorId == request.AuthorId && !a.IsDeleted, cancellationToken);
            if (author==null)
            {
                return Result<int>.Failure(ResultStatus.NotFound,"Author not found.");
            }
            var nameExist=await unitOfWork.Authors.Query()
                .AnyAsync(x => x.Name == request.Name && x.AuthorId != request.AuthorId && !x.IsDeleted, cancellationToken);
           
            if (nameExist)
               return Result<int>.Failure(ResultStatus.Failure, "Author name already exist");

            author.Name=request.Name;
            author.Biography=request.Biography;
            author.DateOfBirth=request.DateOfBirth;
            author.UpdatedAt = DateTime.Now;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(author.AuthorId);
        }
    }
}
