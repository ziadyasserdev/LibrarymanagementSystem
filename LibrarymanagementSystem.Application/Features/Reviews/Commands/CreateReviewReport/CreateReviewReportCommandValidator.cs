using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Commands.CreateReviewReport
{
    public class CreateReviewReportCommandValidator : AbstractValidator<CreateReviewReportCommand>
    {
        public CreateReviewReportCommandValidator()
        {
            RuleFor(x => x.ReviewId)
                .GreaterThan(0)
                .WithMessage("ReviewId must be greater than 0.");

            RuleFor(x => x.Reason)
                .NotEmpty()
                .MaximumLength(500)
                .WithMessage("Reason is required and must be less than 500 characters.");
        }
    }
}
