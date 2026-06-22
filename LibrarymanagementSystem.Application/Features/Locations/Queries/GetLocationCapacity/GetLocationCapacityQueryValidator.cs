using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.GetLocationCapacity
{
    public class GetLocationCapacityQueryValidator
          : AbstractValidator<GetLocationCapacityQuery>
    {
        public GetLocationCapacityQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Location Id must be greater than 0.");
        }
    }
}
