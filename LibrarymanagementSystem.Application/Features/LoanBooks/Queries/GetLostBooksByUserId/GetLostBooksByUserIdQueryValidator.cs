using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetLostBooksByUserId
{
    public class GetLostBooksByUserIdQueryValidator : AbstractValidator<GetLateBooksByUserIdQuery>
    {
        public GetLostBooksByUserIdQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId must not be empty.");
        }
    }
}
