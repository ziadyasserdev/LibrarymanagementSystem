using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Queries.SearchBookCopies
{
    public class SearchBookCopiesQueryValidator : AbstractValidator<SearchBookCopiesQuery>
    {
        public SearchBookCopiesQueryValidator()
        {
          
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(100)
                .WithMessage("PageSize cannot exceed 100.");

          
            RuleFor(x => x.SearchTerm)
                .MaximumLength(200)
                .WithMessage("SearchTerm cannot exceed 200 characters.");

       
            RuleFor(x => x.BookId)
                .GreaterThan(0)
                .When(x => x.BookId.HasValue)
                .WithMessage("BookId must be greater than 0.");

            RuleFor(x => x.LocationId)
                .GreaterThan(0)
                .When(x => x.LocationId.HasValue)
                .WithMessage("LocationId must be greater than 0.");

            RuleFor(x => x.BranchId)
                .GreaterThan(0)
                .When(x => x.BranchId.HasValue)
                .WithMessage("BranchId must be greater than 0.");
        }
    }
}
