using FluentValidation;
using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.RestoreReview
{
    public class RestoreReviewQueryValidator
          : AbstractValidator<RestoreReviewQuery>
    {
        public RestoreReviewQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Review Id must be greater than 0.");
        }
    }
}
