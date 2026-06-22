using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Commands.RestoreBranch
{
    public class RestoreBranchCommandValidator : AbstractValidator<RestoreBranchCommand>
    {
        public RestoreBranchCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Branch Id must be greater than 0");
        }
    }
}
