using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Commands.UpdateLocation
{
    public class UpdateLocationCommandValidator : AbstractValidator<UpdateLocationCommand>
    {
        public UpdateLocationCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Location Id must be greater than 0.");

            RuleFor(x => x.Shelf)
                .NotEmpty().WithMessage("Shelf is required.")
                .MaximumLength(50).WithMessage("Shelf cannot exceed 50 characters.");

            RuleFor(x => x.Floor)
                .NotEmpty().WithMessage("Floor is required.")
                .MaximumLength(50).WithMessage("Floor cannot exceed 50 characters.");
            RuleFor(x => x.Capacity)
               .GreaterThan(0).WithMessage("Capacity must be greater than 0.");
            RuleFor(x => x.Section)
                .NotEmpty().WithMessage("Section is required.")
                .MaximumLength(100).WithMessage("Section cannot exceed 100 characters.");
        }
    }
}
