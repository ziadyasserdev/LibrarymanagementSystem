using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.GetBookRatingSummary
{
    public class GetBookRatingSummaryQueryValidator
          : AbstractValidator<GetBookRatingSummaryQuery>
    {
        public GetBookRatingSummaryQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Book Id must be greater than 0.");
        }
    }
}
