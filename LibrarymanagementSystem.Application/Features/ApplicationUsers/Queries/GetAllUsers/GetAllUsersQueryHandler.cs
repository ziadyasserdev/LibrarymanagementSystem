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
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.ApplicationUsers.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<List<ApplicationUserDto>>>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public GetAllUsersQueryHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Result<List<ApplicationUserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users =await  userManager.Users.Include(c=>c.UserRoles)
                .ThenInclude(c=>c.Role)
                .ToListAsync();
            if(users == null || !users.Any())
                return Result<List<ApplicationUserDto>>.Failure(ResultStatus.NotFound, "No users found.");
            var usersDto = users.Select(c => new ApplicationUserDto
            {
                Id= c.Id,
                FullName= c.FullName,
                Email= c.Email,
                Gender=c.Gender.ToString(),
                PhoneNumber= c.PhoneNumber,
                UserName=c.UserName,
                Roles=c.UserRoles.Select(c=>c.Role.Name).ToList()

            }).ToList();
           
        
           
            

            return Result<List<ApplicationUserDto>>
                .Success(usersDto, "Users retrieved successfully.");

           
        }
    }
}
