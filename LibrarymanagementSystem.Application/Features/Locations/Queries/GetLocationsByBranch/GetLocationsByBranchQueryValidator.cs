using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.GetLocationsByBranch
{
    public class GetLocationsByBranchQueryValidator
       : AbstractValidator<GetLocationsByBranchQuery>
    {
        public GetLocationsByBranchQueryValidator()
        {
            RuleFor(x => x.BranchId)
                .GreaterThan(0)
                .WithMessage("BranchId must be greater than 0");
        }
    }
}
