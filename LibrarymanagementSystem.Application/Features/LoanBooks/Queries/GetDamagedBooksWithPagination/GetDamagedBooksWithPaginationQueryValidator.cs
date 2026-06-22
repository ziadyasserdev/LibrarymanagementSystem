using FluentValidation;
using LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetAllLoanBookWithPagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetDamagedBooksWithPagination
{
    public class GetDamagedBooksWithPaginationQueryValidator
       : AbstractValidator<GetDamagedBooksWithPaginationQuery>
    {
        public GetDamagedBooksWithPaginationQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than zero.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than zero.")
                .LessThanOrEqualTo(50)
                .WithMessage("PageSize cannot exceed 50.");
        }
    }
}
