using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.ApplicationUsers.Dtos;
using LibrarymanagementSystem.Application.Features.Categories.Dtos;
using LibrarymanagementSystem.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.ApplicationUsers.Queries.GetAllUsersWithPagination
{
    public class GetAllUsersWithPaginationQueryHandler : IRequestHandler<GetAllUsersWithPaginationQuery, Result<PaginatedResult<ApplicationUserDto>>>
    {
        private readonly UserManager<ApplicationUser> userManager;
    
        public GetAllUsersWithPaginationQueryHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Result<PaginatedResult<ApplicationUserDto>>> Handle(GetAllUsersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = userManager.Users.Include(c=>c.UserRoles)
                .ThenInclude(x=>x.Role)
                .AsQueryable();

            
            var totalCount = await query.CountAsync(cancellationToken);

          
            var users = await query
                .OrderBy(x => x.FullName)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

           
            var items =users.Select( c => new ApplicationUserDto
            {
                Id = c.Id,
                FullName = c.FullName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                UserName = c.UserName,
                Gender = c.Gender.ToString(),
                Roles = c.UserRoles.Select(r =>r.Role.Name).ToList()!
            });
        
            var paginatedResult = new PaginatedResult<ApplicationUserDto>(items.ToList(), request.PageNumber, request.PageSize, totalCount);
            return Result<PaginatedResult<ApplicationUserDto>>.Success(paginatedResult);
        }
    }
}
