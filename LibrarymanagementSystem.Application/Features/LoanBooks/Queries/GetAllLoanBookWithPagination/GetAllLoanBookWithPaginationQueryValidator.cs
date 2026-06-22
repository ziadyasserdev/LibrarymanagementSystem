using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetAllLoanBookWithPagination
{
    public class GetAllLoanBookWithPaginationQueryValidator
       : AbstractValidator<GetAllLoanBookWithPaginationQuery>
    {
        public GetAllLoanBookWithPaginationQueryValidator()
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
