using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.GetAllReviewsWithPagination
{
    public class GetAllReviewsWithPaginationQueryValidator
        : AbstractValidator<GetAllReviewsWithPaginationQuery>
    {
        private readonly List<string> allowedSortFields = new() { "CreatedAt", "Rating" };

        public GetAllReviewsWithPaginationQueryValidator()
        {
          
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("PageNumber must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("PageSize cannot exceed 100.");

          
            RuleFor(x => x.BookId)
                .GreaterThan(0).WithMessage("BookId must be greater than 0.")
                .When(x => x.BookId.HasValue);

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId cannot be empty.")
                .When(x => !string.IsNullOrWhiteSpace(x.UserId));

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage("Rating must be between 1 and 5.")
                .When(x => x.Rating.HasValue);

         
            RuleFor(x => x.OrderBy)
                .Must(field => string.IsNullOrEmpty(field) || allowedSortFields.Contains(field))
                .WithMessage($"OrderBy must be one of: {string.Join(", ", allowedSortFields)}.");
        }
    }
}