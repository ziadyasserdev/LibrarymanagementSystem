using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.GetLocationById
{
    public class GetLocationByIdQueryValidator : AbstractValidator<GetLocationByIdQuery>
    {
        public GetLocationByIdQueryValidator()
        {
            RuleFor(x => x.LocationId)
                .GreaterThan(0)
                .WithMessage("LocationId must be greater than 0.");
        }
    }
}
