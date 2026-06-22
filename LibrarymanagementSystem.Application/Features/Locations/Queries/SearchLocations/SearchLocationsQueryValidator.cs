using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.SearchLocations
{
    public class SearchLocationsQueryValidator : AbstractValidator<SearchLocationsQuery>
    {
        public SearchLocationsQueryValidator()
        {
            RuleFor(x => x.Shelf)
                .MaximumLength(50)
                .WithMessage("Shelf name cannot exceed 50 characters");

            RuleFor(x => x.Floor)
                .MaximumLength(50)
                .WithMessage("Floor name cannot exceed 50 characters");

            RuleFor(x => x.Section)
                .MaximumLength(50)
                .WithMessage("Section name cannot exceed 50 characters");

            RuleFor(x => x.BranchId)
                .GreaterThan(0)
                .When(x => x.BranchId.HasValue)
                .WithMessage("BranchId must be greater than zero");
        }
    }
}
