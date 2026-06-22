using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Queries.GetBookCopiesWithPagination
{
    public class GetBookCopiesWithPaginationQueryValidator
        : AbstractValidator<GetBookCopiesWithPaginationQuery>
    {
        public GetBookCopiesWithPaginationQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageNumber must be greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageSize must be greater than or equal to 1.")
                .LessThanOrEqualTo(50)
                .WithMessage("PageSize cannot exceed 50.");
        }
    }
}
