using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Authorizations.Dtos;
using LibrarymanagementSystem.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authorizations.Queries.GetAllRolesWithPagination
{
    public class GetAllRolesWithPaginationQueryHandler : IRequestHandler<GetAllRolesWithPaginationQuery, Result<PaginatedResult<RoleDto>>>
    {
        private readonly RoleManager<ApplicationRole> roleManager;

        public GetAllRolesWithPaginationQueryHandler(RoleManager<ApplicationRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        public async Task<Result<PaginatedResult<RoleDto>>> Handle(GetAllRolesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query =roleManager.Roles.AsQueryable();
            var rolesCount = await query.CountAsync();
            var items =await query.OrderBy(x => x.Name)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(c => new RoleDto
                {
                    Id = c.Id,
                    Name=c.Name!
                }).ToListAsync();
            var paginatedResult = 
                new PaginatedResult<RoleDto>(items, request.PageNumber, request.PageSize, rolesCount);
            return Result<PaginatedResult<RoleDto>>.Success(paginatedResult);
        }
    }
}
