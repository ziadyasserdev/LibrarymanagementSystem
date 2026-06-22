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

namespace LibrarymanagementSystem.Application.Features.Branches.Queries.GetAllBranchesForAdmin
{

    public class GetAllBranchesForAdminQueryHandler : IRequestHandler<GetAllBranchesForAdminQuery, Result<List<AdminBranchDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllBranchesForAdminQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<AdminBranchDto>>> Handle(GetAllBranchesForAdminQuery request, CancellationToken cancellationToken)
        {
            var branches=await unitOfWork.Branches.Query()
                .Select(x => new AdminBranchDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Address = x.Address,
                    CreatedAt = x.CreatedAt,
                    IsActive = x.IsActive,
                    Phone=x.PhoneNumber,
                }).ToListAsync();
            return Result<List<AdminBranchDto>>.Success(branches);
        }
    }
}
