using FluentValidation;
using LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetLostBooksByUserId;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetAllLateBooksByUserId
{
    public class GetAllLateBooksByUserIdValidator : AbstractValidator<GetLateBooksByUserIdQuery>
    {
        public GetAllLateBooksByUserIdValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId must not be empty.");
        }
    }
}
