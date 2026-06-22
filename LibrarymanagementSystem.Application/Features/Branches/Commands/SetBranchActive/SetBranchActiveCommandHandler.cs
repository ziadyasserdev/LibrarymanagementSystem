using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Commands.SetBranchActive
{
    public class SetBranchActiveCommandHandler : IRequestHandler<SetBranchActiveCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public SetBranchActiveCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(SetBranchActiveCommand request, CancellationToken cancellationToken)
        {
           var branch=await unitOfWork.Branches.Query()
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            if (branch == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Branch not found");
            if(branch.IsActive)
                return Result<int>.Failure(ResultStatus.Conflict, "Branch already active");
            branch.IsActive = true; 
            branch.UpdatedAt = DateTime.Now;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(branch.Id);  
        }
    }
}
