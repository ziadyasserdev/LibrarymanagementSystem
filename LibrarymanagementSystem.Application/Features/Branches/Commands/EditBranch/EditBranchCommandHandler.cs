using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Branches.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Commands.EditBranch
{
    public class EditBranchCommandHandler : IRequestHandler<EditBranchCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public EditBranchCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
   
        public async Task<Result<int>> Handle(EditBranchCommand request, CancellationToken cancellationToken)
        {
            var branch = await unitOfWork.Branches.Query()
          .FirstOrDefaultAsync(b => b.Id == request.Id && !b.IsDeleted , cancellationToken);

            if (branch == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Branch not found");

            var existingBranch = await unitOfWork.Branches.Query()
       .Where(x => !x.IsDeleted && x.Id != request.Id
           && (x.Name == request.Name || x.Code == request.Code))
       .Select(x => new { x.Name, x.Code })
       .FirstOrDefaultAsync(cancellationToken);
            if (existingBranch != null)
            {
                if (existingBranch.Name == request.Name)
                    return Result<int>.Failure(ResultStatus.Failure, "Branch name already exists");

                if (existingBranch.Code == request.Code)
                    return Result<int>.Failure(ResultStatus.Failure, "Branch code already exists");
            }
            branch.Name= request.Name;
            branch.Code= request.Code;
            branch.Email= request.Email;
            branch.PhoneNumber= request.PhoneNumber;
            branch.Address= request.Address;
            branch.City= request.City;
            branch.UpdatedAt = DateTime.Now;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(branch.Id);

        }
    }
}
