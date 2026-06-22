using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Commands.DeleteBranch
{
    public class DeleteBranchCommandHandler : IRequestHandler<DeleteBranchCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeleteBranchCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
        {
           var branch=await unitOfWork.Branches.Query()
                .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted);
            if(branch == null)
                return Result<int>.Failure(ResultStatus.NotFound,"Branch not found");

            var hasLocations = await unitOfWork.Locations.Query()
                .AnyAsync(l => l.BranchId == request.Id && !l.IsDeleted, cancellationToken);

            if (hasLocations)
                return Result<int>.Failure(ResultStatus.Failure, "Cannot delete branch with assigned locations");

           

            branch.IsDeleted = true;
            branch.IsActive = false;
            branch.DeletedAt = DateTime.Now;
            branch.UpdatedAt = DateTime.Now;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(branch.Id);
        }
    }
}
