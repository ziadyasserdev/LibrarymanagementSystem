using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Commands.DeactivateBranchesBulk
{
    public class DeactivateBranchesBulkCommandValidator
        : AbstractValidator<DeactivateBranchesBulkCommand>
    {
        public DeactivateBranchesBulkCommandValidator()
        {
            RuleFor(x => x.BranchesId)
                .NotNull()
                .WithMessage("BranchesId list cannot be null")
                .NotEmpty()
                .WithMessage("BranchesId list cannot be empty");

            RuleForEach(x => x.BranchesId)
                .GreaterThan(0)
                .WithMessage("Branch Id must be greater than 0");

            RuleFor(x => x.BranchesId)
                .Must(ids => ids.Distinct().Count() == ids.Count)
                .WithMessage("Duplicate Branch Ids are not allowed");
        }
    }
}
