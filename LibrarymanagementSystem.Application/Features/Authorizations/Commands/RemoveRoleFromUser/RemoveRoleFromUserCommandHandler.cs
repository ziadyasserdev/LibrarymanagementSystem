using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authorizations.Commands.RemoveRoleFromUser
{
    public class RemoveRoleFromUserCommandHandler : IRequestHandler<RemoveRoleFromUserCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public RemoveRoleFromUserCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<Result<string>> Handle(RemoveRoleFromUserCommand request, CancellationToken cancellationToken)
        {
            var user=await userManager.FindByIdAsync(request.userId);
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
                    $"Role '{role.Name}' is inactive"
                );

            if (!await userManager.IsInRoleAsync(user, role.Name!))
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    $"User already is not in this role."
                );
            var result=await userManager.RemoveFromRoleAsync(user,role.Name!);

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
                $"Role '{role.Name}' removed from user successfully."
            );
        }
    }
}
