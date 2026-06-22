using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Queries.GetBookCopiesByBook
{
    public class GetBookCopiesByBookQueryValidator : AbstractValidator<GetBookCopiesByBookQuery>
    {
        public GetBookCopiesByBookQueryValidator()
        {
            RuleFor(x => x.BookId)
                .GreaterThan(0)
                .WithMessage("BookId must be greater than 0.");

            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("PageSize must be between 1 and 100.");

            RuleFor(x => x.SortBy)
                .MaximumLength(50)
                .WithMessage("SortBy cannot exceed 50 characters.");

        }
    }
}