using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Commands.DeactivateBranchesBulk
{
    public class DeactivateBranchesBulkCommandHandler : IRequestHandler<DeactivateBranchesBulkCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeactivateBranchesBulkCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(DeactivateBranchesBulkCommand request, CancellationToken cancellationToken)
        {
            
            var ids = request.BranchesId.Distinct().ToList();

            var branches = await unitOfWork.Branches.Query()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(cancellationToken);

            if (!branches.Any())
                return Result<string>
                    .Failure(ResultStatus.NotFound, "Branches not found");

            var validBranches = branches
                .Where(x => !x.IsDeleted)
                .ToList();

            if (!validBranches.Any())
                return Result<string>
                    .Failure(ResultStatus.Failure,
                        "All selected branches are deleted");

            var toDeactivate = validBranches
                .Where(x => x.IsActive)
                .ToList();

            if (!toDeactivate.Any())
            {
                var names = validBranches.Select(x => x.Name);
                return Result<string>
                    .Failure(ResultStatus.Failure,
                        $"Branches already deactivated: {string.Join(", ", names)}");
            }

            var utcNow = DateTime.Now;

            foreach (var branch in toDeactivate)
            {
                branch.IsActive = false;
                branch.UpdatedAt = utcNow;
            }

            await unitOfWork.SaveAsync();

            return Result<string>
                .Success($"{toDeactivate.Count} branches deactivated successfully");
        }
    }
}
