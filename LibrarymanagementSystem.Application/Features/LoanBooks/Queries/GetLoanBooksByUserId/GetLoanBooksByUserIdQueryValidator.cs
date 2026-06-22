using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetLoanBooksByUserId
{
    public class GetLoanBooksByUserIdQueryValidator
        : AbstractValidator<GetLoanBooksByUserIdQuery>
    {
        public GetLoanBooksByUserIdQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.")
                .NotNull()
                .WithMessage("UserId cannot be null.")
                .Must(id => !string.IsNullOrWhiteSpace(id))
                .WithMessage("UserId cannot be empty or whitespace.")
                .MaximumLength(450)
                .WithMessage("UserId length is invalid.");
        }
    }
}
