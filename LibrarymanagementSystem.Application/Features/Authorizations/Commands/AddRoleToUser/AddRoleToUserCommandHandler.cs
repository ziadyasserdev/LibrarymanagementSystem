using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Authorizations.Dtos;
using LibrarymanagementSystem.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authorizations.Commands.AddRoleToUser
{
    public class AddRoleToUserCommandHandler : IRequestHandler<AddRoleToUserCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public AddRoleToUserCommandHandler(UserManager<ApplicationUser> userManager,RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<Result<string>> Handle(AddRoleToUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.userId);
            if (user == null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    $"User with ID '{request.userId}' was not found."
                );

            var role = await roleManager.FindByIdAsync(request.roleId);
            if (role == null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    $"Role with ID '{request.roleId}' was not found."
                );

            if (!role.IsActive)
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    $"Role '{role.Name}' is inactive and cannot be assigned."
                );

            if (await userManager.IsInRoleAsync(user, role.Name!))
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    $"User is already assigned to role '{role.Name}'."
                );

            var result = await userManager.AddToRoleAsync(user, role.Name!);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));

                return Result<string>.Failure(
                    ResultStatus.Failure,
                    $"Role assignment failed. Details: {errorMessage}"
                );
            }

            return Result<string>.Success(
                user.Id,
                $"Role '{role.Name}' assigned to user successfully."
            );
        }
    }
}
