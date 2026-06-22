using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Commands.RestoreBranchesBulk
{
    public class RestoreBranchesBulkCommandHandler : IRequestHandler<RestoreBranchesBulkCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public RestoreBranchesBulkCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(RestoreBranchesBulkCommand request, CancellationToken cancellationToken)
        {
            var branchesId=request.BranchIds.Distinct().ToList();
            var branches = await unitOfWork.Branches.Query()
                .Where(x => branchesId.Contains(x.Id)).ToListAsync();
            if(!branches.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No branches found");
            var deletedBranches =branches.Where(x => x.IsDeleted).ToList();
            if(!deletedBranches.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "All selected branches are already restored");

            foreach(var branch in deletedBranches)
            {
                branch.IsDeleted = false;
                branch.UpdatedAt = DateTime.UtcNow;
                branch.DeletedAt = null;
                
            }
            await unitOfWork.SaveAsync();
            return Result<string>.Success($"{deletedBranches.Count} branches restored successfully");
        }
    }
}
