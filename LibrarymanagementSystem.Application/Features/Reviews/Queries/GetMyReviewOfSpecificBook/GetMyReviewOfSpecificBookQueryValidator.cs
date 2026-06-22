using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.GetMyReviewOfSpecificBook
{
    public class GetMyReviewOfSpecificBookQueryValidator
        : AbstractValidator<GetMyReviewOfSpecificBookQuery>
    {
        public GetMyReviewOfSpecificBookQueryValidator()
        {
            RuleFor(x => x.BookId)
                .GreaterThan(0)
                .WithMessage("BookId must be greater than 0.");
        }
    }
}
