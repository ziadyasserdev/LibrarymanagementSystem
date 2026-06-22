using FluentValidation;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Commands.PayFine
{
    public class PayFineCommandValidator : AbstractValidator<PayFineCommand>
    {
       

        public PayFineCommandValidator()
        {
           

          
            RuleFor(x => x.FineId)
                .GreaterThan(0)
                .WithMessage("FineId must be greater than zero.")
               ;

          
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than zero.");

           
            RuleFor(x => x.PaymentMethod)
                .IsInEnum()
                .WithMessage("Invalid payment method.");

          
            RuleFor(x => x.PayStatus)
                .IsInEnum()
                .WithMessage("Invalid pay status.");
        }
    }
}
