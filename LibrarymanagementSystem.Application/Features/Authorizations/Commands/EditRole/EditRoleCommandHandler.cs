using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Authorizations.Dtos;
using LibrarymanagementSystem.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authorizations.Commands.EditRole
{
    public class EditRoleCommandHandler : IRequestHandler<EditRoleCommand, Result<RoleDto>>
    {
        private readonly RoleManager<ApplicationRole> roleManager;

        public EditRoleCommandHandler(RoleManager<ApplicationRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        public async Task<Result<RoleDto>> Handle(EditRoleCommand request, CancellationToken cancellationToken)
        {
           
            var role=await roleManager.FindByIdAsync(request.roleId);
            if (role == null)
                return Result<RoleDto>.Failure(ResultStatus.Failure, "Role not found");
            var existingRole = await roleManager.FindByNameAsync(request.newRoleName);

            if (existingRole != null && existingRole.Id != request.roleId)
            {
                return Result<RoleDto>.Failure(
                    ResultStatus.Failure,
                    "Role name already exists."
                );
            }
            role.Name = request.newRoleName;
           var result= await roleManager.UpdateAsync(role);
            if(!result.Succeeded)
            {
                var errors=result.Errors.Select(x=>x.Description).ToList();
                return Result<RoleDto>.Failure(ResultStatus.Failure, $"Failed to update role. Reasons:{errors}");
            }
            var roleDto = new RoleDto
            {
                Id=request.roleId,
                Name=request.newRoleName,
            };
            return Result<RoleDto>.Success(roleDto, "Role updated successfully.");
        }
    }
}
