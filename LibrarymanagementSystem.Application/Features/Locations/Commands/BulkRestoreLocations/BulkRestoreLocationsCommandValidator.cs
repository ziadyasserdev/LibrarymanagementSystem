using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Commands.BulkRestoreLocations
{
    public class BulkRestoreLocationsCommandValidator:AbstractValidator<BulkRestoreLocationsCommand>
    {
        public BulkRestoreLocationsCommandValidator()
        {
            RuleFor(x => x.LocationIds)
             .NotNull()
             .WithMessage("LocationIds list cannot be null.")
             .NotEmpty()
             .WithMessage("LocationIds list cannot be empty.");

            RuleForEach(x => x.LocationIds)
                .GreaterThan(0)
                .WithMessage("Each LocationId must be greater than 0.");
        }
    }
}
