using FluentValidation;
using LibrarymanagementSystem.Data.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authorizations.Commands.CreateRole
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        private readonly RoleManager<ApplicationRole> roleManager;

        public CreateRoleCommandValidator(RoleManager<ApplicationRole> roleManager)
        {
            RuleFor(x => x.roleName)
                .NotEmpty().WithMessage("Role name is required.")
                .MinimumLength(3).WithMessage("Role name must be at least 3 characters.")
                .MaximumLength(10).WithMessage("Role name must not exceed 10 characters.")
               ;
            this.roleManager = roleManager;
        }
        
    }
}
