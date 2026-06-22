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

namespace LibrarymanagementSystem.Application.Features.Branches.Queries.GetBranchById
{
    public class GetBranchByIdQueryHandler : IRequestHandler<GetBranchByIdQuery, Result<AdminBranchDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetBranchByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<AdminBranchDto>> Handle(GetBranchByIdQuery request, CancellationToken cancellationToken)
        {
            var branch=await unitOfWork.Branches.Query()
                .FirstOrDefaultAsync(x => x.Id  == request.Id);
            if (branch == null)
                return Result<AdminBranchDto>.Failure(ResultStatus.NotFound, "Branch not found");
            return Result<AdminBranchDto>.Success(new AdminBranchDto
            {
                Id=request.Id,
                Email=branch.Email,
                Address=branch.Address,
                CreatedAt=branch.CreatedAt,
                IsActive=branch.IsActive,
                Name=branch.Name,
                Phone=branch.PhoneNumber
            });
        }
    }
}
