using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Commands.DeleteBranchesBulk
{
    public class DeleteBranchesBulkCommandHandler : IRequestHandler<DeleteBranchesBulkCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeleteBranchesBulkCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(DeleteBranchesBulkCommand request, CancellationToken cancellationToken)
        {
            var branches = await unitOfWork.Branches.Query()
        .Where(x => request.BranchIds.Contains(x.Id) && !x.IsDeleted)
        .ToListAsync(cancellationToken);

            if (!branches.Any())
                return Result<string>
                    .Failure(ResultStatus.NotFound, "Branches not found");

           

            foreach (var branch in branches)
            {
                branch.IsDeleted = true;
                branch.IsActive = false;
                branch.DeletedAt = DateTime.Now;
                branch.UpdatedAt = DateTime.Now;
            }

            await unitOfWork.SaveAsync();

            return Result<string>
                .Success("Branches deleted successfully");
        } 
    }
}
