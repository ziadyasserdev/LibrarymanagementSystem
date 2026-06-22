using FluentValidation;
using LibrarymanagementSystem.Data.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authorizations.Commands.ToggleRoleStatus
{
    public class ToggleRoleStatusCommandValidator : AbstractValidator<ToggleRoleStatusCommand>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ToggleRoleStatusCommandValidator(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;

          
            RuleFor(x => x.roleId)
                .NotEmpty().WithMessage("Role Id is required.")
              ;
        }

     
    }
}
