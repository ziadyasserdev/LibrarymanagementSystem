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

namespace LibrarymanagementSystem.Application.Features.Authorizations.Commands.DeleteRole
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Result<string>>
    {
        private readonly RoleManager<ApplicationRole> roleManager;

        public DeleteRoleCommandHandler(RoleManager<ApplicationRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        public async Task<Result<string>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await roleManager.FindByIdAsync(request.RoleId);
            if(role == null)
                return Result<string>.Failure(ResultStatus.Failure, "Role not found");
            role.IsActive = false;
            var result = await roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);
                return Result<string>.Failure(ResultStatus.Failure, $"Role creation failed.{errors.ToList()}");
            }

            return Result<string>.Success(role.Id);
        }
    }
}
