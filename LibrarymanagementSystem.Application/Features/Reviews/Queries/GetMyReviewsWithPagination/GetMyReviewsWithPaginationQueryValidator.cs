using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.GetMyReviewsWithPagination
{
    public class GetMyReviewsWithPaginationQueryValidator
    : AbstractValidator<GetMyReviewsWithPaginationQuery>
    {
        public GetMyReviewsWithPaginationQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(50)
                .WithMessage("PageSize must be between 1 and 50.");

            RuleFor(x => x.ReviewFilter)
                .IsInEnum()
                .WithMessage("Invalid ReviewFilter value.");
        }
    }
}
