using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Queries.SearchBranches
{
    public class SearchBranchesValidator
         : AbstractValidator<SearchBranchesQuery>
    {
        public SearchBranchesValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(50);

            RuleFor(x => x.SearchTerm)
                .MaximumLength(100);

            RuleFor(x => x.City)
                .MaximumLength(50);
        }
    }
}
