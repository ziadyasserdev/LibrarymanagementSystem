using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetAllFinePaymentsOfUser
{
    public class GetAllFinePaymentsOfUserQueryValidator
       : AbstractValidator<GetAllFinePaymentsOfUserQuery>
    {
        public GetAllFinePaymentsOfUserQueryValidator()
        {
            RuleFor(x => x.userId)
                .NotEmpty()
                .WithMessage("User Id is required.")
                .MaximumLength(450)
                .WithMessage("Invalid User Id.");
        }
    }
}
