using FluentValidation;
using LibrarymanagementSystem.Application.Features.Branches.Commands.SetBranchActive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Commands.DeactivateBranch
{
    public class DeactivateBranchCommandValidator : AbstractValidator<DeactivateBranchCommand>
    {
        public DeactivateBranchCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Branch Id must be greater than 0");


        }
    }
}
