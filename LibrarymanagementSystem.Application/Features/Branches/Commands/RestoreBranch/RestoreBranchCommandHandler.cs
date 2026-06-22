using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Commands.RestoreBranch
{
    public class RestoreBranchCommandHandler : IRequestHandler<RestoreBranchCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public RestoreBranchCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(RestoreBranchCommand request, CancellationToken cancellationToken)
        {
            var branch = await unitOfWork.Branches.Query()
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsDeleted);
            if (branch == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Branch not found");
            branch.IsDeleted = false;
            branch.DeletedAt = null;
            branch.UpdatedAt = DateTime.Now;
            
            await unitOfWork.SaveAsync();
            return Result<int>.Success(branch.Id);
        }
    }
}
