using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Commands.CreateReview
{
    
        public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
        {
            public CreateReviewCommandValidator()
            {
                RuleFor(x => x.Rating)
                    .NotEmpty().WithMessage("Rating is required.")
                    .InclusiveBetween(1, 5)
                    .WithMessage("Rating must be between 1 and 5.");

                RuleFor(x => x.BookId)
                    .NotEmpty().WithMessage("BookId is required.")
                    .GreaterThan(0)
                    .WithMessage("BookId must be greater than 0.");

                RuleFor(x => x.Comment)
                    .MaximumLength(500)
                    .WithMessage("Comment cannot exceed 500 characters.")
                    .When(x => !string.IsNullOrWhiteSpace(x.Comment));
            }
        }
    }
