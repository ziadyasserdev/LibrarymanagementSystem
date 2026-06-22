using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.CheckUserHasReviewed
{
    public class CheckUserHasReviewedQueryValidator : AbstractValidator<CheckUserHasReviewedQuery>
    {
        public CheckUserHasReviewedQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("BookId must be greater than 0.");
        }
    }
}
