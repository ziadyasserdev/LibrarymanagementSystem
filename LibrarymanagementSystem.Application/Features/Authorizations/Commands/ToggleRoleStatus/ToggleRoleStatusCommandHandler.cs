using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authorizations.Commands.ToggleRoleStatus
{
    public class ToggleRoleStatusCommandHandler : IRequestHandler<ToggleRoleStatusCommand, Result<string>>
    {
        private readonly RoleManager<ApplicationRole> roleManager;

        public ToggleRoleStatusCommandHandler(RoleManager<ApplicationRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        public async Task<Result<string>> Handle(ToggleRoleStatusCommand request, CancellationToken cancellationToken)
        {
            var role=await roleManager.FindByIdAsync(request.roleId);
            if (role == null)
                if (role == null)
                    return Result<string>.Failure(ResultStatus.Failure, "Role not found");
            role.IsActive = true;
            var result = await roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);
                return Result<string>.Failure(ResultStatus.Failure, $"Role status update failed:{errors.ToList()}");
            }

            return Result<string>.Success(role.Id);
        }
    }
}
