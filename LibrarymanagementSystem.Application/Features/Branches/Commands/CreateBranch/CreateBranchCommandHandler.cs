using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Branches.Dtos;
using LibrarymanagementSystem.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Commands.CreateBranch
{
    public class CreateBranchCommandHandler : IRequestHandler<CreateBranchCommand, Result<CreateBranchDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public CreateBranchCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
      
        public async Task<Result<CreateBranchDto>> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Branches.Query();

            var existingBranch = await query
                .Where(x => !x.IsDeleted &&
                           (x.Name == request.Name || x.Code == request.Code))
                .Select(x => new { x.Name, x.Code })
                .FirstOrDefaultAsync(cancellationToken);

            if (existingBranch != null)
            {
                if (existingBranch.Name == request.Name)
                    return Result<CreateBranchDto>
                        .Failure(ResultStatus.Failure, "Branch name already exists");

                if (existingBranch.Code == request.Code)
                    return Result<CreateBranchDto>
                        .Failure(ResultStatus.Failure, "Branch code already exists");
            }

            var branch = new Branch
            {
                Name = request.Name.Trim(),
                Code = request.Code.Trim(),
                Address = request.Address,
                City = request.City,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            await unitOfWork.Branches.AddAsync(branch);
            await unitOfWork.SaveAsync();

            var dto = new CreateBranchDto
            {
                Id = branch.Id,
                Name = branch.Name,
                Code = branch.Code,
                Address = branch.Address,
                City = branch.City,
                PhoneNumber = branch.PhoneNumber,
                Email = branch.Email,
                CreatedAt = branch.CreatedAt
            };

            return Result<CreateBranchDto>.Success(dto);
        }
    }
}
