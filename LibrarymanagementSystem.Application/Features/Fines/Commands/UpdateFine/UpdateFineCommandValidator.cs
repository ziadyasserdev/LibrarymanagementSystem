using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Commands.UpdateFine
{
    public class UpdateFineCommandValidator : AbstractValidator<UpdateFineCommand>
    {
        public UpdateFineCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Fine Id must be greater than 0.");

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0)
                .WithMessage("TotalAmount must be greater than 0.");

            RuleFor(x => x.Notes)
                .MaximumLength(500)
                .WithMessage("Notes cannot exceed 500 characters.");
        }
    }
}
