using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Branches.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Queries.GetAllBranchesWithPaginationForAdmin
{
    public class GetAllBranchesWithPaginationForAdminQueryHandler : IRequestHandler<GetAllBranchesWithPaginationForAdminQuery, Result<PaginatedResult<AdminBranchDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllBranchesWithPaginationForAdminQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<PaginatedResult<AdminBranchDto>>> Handle(GetAllBranchesWithPaginationForAdminQuery request, CancellationToken cancellationToken)
        {
            var query =unitOfWork.Branches.Query()
                .AsNoTracking();
            switch (request.BranchStatus)
            {
                case BranchStatus.All:
                    query=query.AsNoTracking();
                    break;
                case BranchStatus.Active:
                    query=query.Where(x => x.IsActive);
                    break;
                case BranchStatus.InActive:
                    query=query.Where(x => !x.IsActive);
                    break;
            }

            var totalCount=await query.CountAsync();
            var items=await query.OrderByDescending(x => x.CreatedAt)
                .Skip((request.PageNumber-1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new AdminBranchDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Address = x.Address,
                    CreatedAt = x.CreatedAt,
                    IsActive = x.IsActive,
                    Phone = x.PhoneNumber,
                }).ToListAsync();
            return Result<PaginatedResult<AdminBranchDto>>.Success(new PaginatedResult<AdminBranchDto>(items, request.PageNumber, request.PageSize, totalCount));
        }
    }
}
