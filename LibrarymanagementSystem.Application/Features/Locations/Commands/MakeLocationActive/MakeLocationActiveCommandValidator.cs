using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Commands.MakeLocationActive
{
    public class MakeLocationActiveCommandValidator : AbstractValidator<MakeLocationActiveCommand>
    {
        public MakeLocationActiveCommandValidator()
        {
            RuleFor(x => x.LocationId)
                .NotEmpty().WithMessage("LocationId is required.")
                .GreaterThan(0).WithMessage("LocationId must be greater than 0.");
        }
    }
}
