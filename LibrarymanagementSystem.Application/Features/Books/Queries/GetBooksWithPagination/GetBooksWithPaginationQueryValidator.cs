using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksWithPagination
{
    public class GetBooksWithPaginationQueryValidator : AbstractValidator<GetBooksWithPaginationQuery>
    {
        public GetBooksWithPaginationQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than zero.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("Page size must be greater than zero.")
                .LessThanOrEqualTo(100) 
                .WithMessage("Page size cannot exceed 100.");
        }
    }
}
