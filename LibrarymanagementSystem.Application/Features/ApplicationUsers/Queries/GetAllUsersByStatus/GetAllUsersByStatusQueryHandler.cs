using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.ApplicationUsers.Dtos;
using LibrarymanagementSystem.Data.Enum;
using LibrarymanagementSystem.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.ApplicationUsers.Queries.GetAllUsersByStatus
{
    public class GetAllUsersByStatusQueryHandler : IRequestHandler<GetAllUsersByStatusQuery, Result<PaginatedResult<ApplicationUserDto>>>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public GetAllUsersByStatusQueryHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Result<PaginatedResult<ApplicationUserDto>>> Handle(GetAllUsersByStatusQuery request, CancellationToken cancellationToken)
        {
           var query=userManager.Users
                .AsNoTracking()
                .AsQueryable();
            switch (request.UserStatus)
            {
                case UserStatus.Active:
                    query = query.Where(x => !x.IsDeleted);
                    break;
                case UserStatus.InActive:
                    query = query.Where(x => x.IsDeleted);
                    break;
            }
            var totalCount = await query.CountAsync();
            var users = await query.OrderBy(x => x.FullName)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ApplicationUserDto
                {
                    Id=x.Id,
                    UserName=x.UserName!,
                    FullName=x.FullName,
                    Email=x.Email!,
                    PhoneNumber=x.PhoneNumber!,
                    Gender=x.Gender.ToString(),
                    Roles = x.UserRoles.Select(r => r.Role.Name).ToList()!
                }).ToListAsync();

            var paginatedResult = new PaginatedResult<ApplicationUserDto>(users, request.PageNumber, request.PageSize, totalCount);
            return Result<PaginatedResult<ApplicationUserDto>>.Success(paginatedResult);
        }
    }
}
