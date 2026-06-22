using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Commands.ActivateBranchesBulk
{
    public class ActivateBranchesBulkCommandHandler : IRequestHandler<ActivateBranchesBulkCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public ActivateBranchesBulkCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(ActivateBranchesBulkCommand request, CancellationToken cancellationToken)
        {
            var branches = await unitOfWork.Branches.Query()
              .Where(x => request.BranchesId.Contains(x.Id))
              .ToListAsync();
            if (!branches.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No branches were found for the provided IDs.");
            var allNotActive = branches
    .Where(x => !x.IsDeleted && !x.IsActive)
    .ToList();

            if (!allNotActive.Any())
                return Result<string>
                    .Failure(ResultStatus.Failure, "All branches are already active or deleted");
            foreach (var branch in allNotActive)
            {
                branch.IsActive = true;
                branch.UpdatedAt = DateTime.Now;
                
            }
            await unitOfWork.SaveAsync();
            return Result<string>.Success($"{allNotActive.Count} branches activated successfully.");
         }
    }
}
