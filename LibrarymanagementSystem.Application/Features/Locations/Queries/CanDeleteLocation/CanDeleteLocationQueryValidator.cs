using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.CanDeleteLocation
{
    public class CanDeleteLocationQueryValidator
       : AbstractValidator<CanDeleteLocationQuery>
    {
        public CanDeleteLocationQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Location Id must be greater than 0.");
        }
    }
}
