using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetLoanBookById
{
    public class GetLoanBookByIdQueryValidator
          : AbstractValidator<GetLoanBookByIdQuery>
    {
        public GetLoanBookByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("LoanBook Id must be greater than zero");
        }
    }
}
