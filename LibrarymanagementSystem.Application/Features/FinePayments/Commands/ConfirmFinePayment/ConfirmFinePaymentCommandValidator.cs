using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Commands.ConfirmFinePayment
{
    public class ConfirmFinePaymentCommandValidator : AbstractValidator<ConfirmFinePaymentCommand>
    {
        public ConfirmFinePaymentCommandValidator()
        {
           
            RuleFor(x => x.FinePaymentId)
                .GreaterThan(0).WithMessage("FinePaymentId must be greater than 0.");

           
            RuleFor(x => x.Success)
                .NotNull().WithMessage("Success must be specified.");
        }
    }
}
