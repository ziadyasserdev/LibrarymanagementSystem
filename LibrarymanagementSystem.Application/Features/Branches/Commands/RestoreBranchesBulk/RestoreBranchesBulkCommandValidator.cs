using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Commands.RestoreBranchesBulk
{
    public class RestoreBranchesBulkCommandValidator:AbstractValidator<RestoreBranchesBulkCommand>
    {
        public RestoreBranchesBulkCommandValidator()
        {
            RuleFor(x => x.BranchIds)
               .NotNull()
               .WithMessage("BranchesId list cannot be null")
               .NotEmpty()
               .WithMessage("BranchesId list cannot be empty");

            RuleForEach(x => x.BranchIds)
                .GreaterThan(0)
                .WithMessage("Branch Id must be greater than 0");

            RuleFor(x => x.BranchIds)
                .Must(ids => ids.Distinct().Count() == ids.Count)
                .WithMessage("Duplicate Branch Ids are not allowed");
        }
    }
}
