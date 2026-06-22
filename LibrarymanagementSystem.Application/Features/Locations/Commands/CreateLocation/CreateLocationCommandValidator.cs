using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Commands.CreateLocation
{

    public class CreateLocationCommandValidator : AbstractValidator<CreateLocationCommand>
    {
        public CreateLocationCommandValidator()
        {
            RuleFor(x => x.Shelf)
                .NotEmpty().WithMessage("Shelf cannot be empty.")
                .MaximumLength(50).WithMessage("Shelf cannot exceed 50 characters.");

            RuleFor(x => x.Floor)
                .NotEmpty().WithMessage("Floor cannot be empty.")
                .MaximumLength(50).WithMessage("Floor cannot exceed 50 characters.");

            RuleFor(x => x.Section)
                .NotEmpty().WithMessage("Section cannot be empty.")
                .MaximumLength(50).WithMessage("Section cannot exceed 50 characters.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Capacity must be greater than 0.");

            RuleFor(x => x.BranchId)
                .GreaterThan(0).WithMessage("BranchId must be a valid ID.");
        }
    }
}


