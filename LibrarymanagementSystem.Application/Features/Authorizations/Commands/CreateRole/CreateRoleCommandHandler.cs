using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Authorizations.Dtos;
using LibrarymanagementSystem.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authorizations.Commands.CreateRole
{

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Result<RoleDto>>
    {
        private readonly RoleManager<ApplicationRole> roleManager;

        public CreateRoleCommandHandler(RoleManager<ApplicationRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        public async Task<Result<RoleDto>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var roleExist = await roleManager.RoleExistsAsync(request.roleName);
            if (roleExist)
                return Result<RoleDto>.Failure(ResultStatus.Failure, "Role Already Exist");
            var role= new ApplicationRole { Name = request.roleName };
          var result= await roleManager.CreateAsync(role);
            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);
                return Result<RoleDto>.Failure(ResultStatus.Failure, $"Role creation failed.{errors.ToList()}");
            }
            var roleDto=new RoleDto { Id = role.Id, Name = role.Name };
            return Result<RoleDto>.Success(roleDto);
        }
    }
}
