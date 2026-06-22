using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetRevenuePayments
{
    public class GetRevenuePaymentsQueryValidator
         : AbstractValidator<GetRevenuePaymentsQuery>
    {
        public GetRevenuePaymentsQueryValidator()
        {
            RuleFor(x => x.From)
           .NotEmpty()
           .WithMessage("From date is required.");

            RuleFor(x => x.To)
                .NotEmpty()
                .WithMessage("To date is required.");

            RuleFor(x => x.To)
                .GreaterThanOrEqualTo(x => x.From)
                .WithMessage("'To' date must be greater than or equal to 'From' date.");

            RuleFor(x => x)
                .Must(x => (x.To - x.From).TotalDays <= 365)
                .When(x => x.To >= x.From)
                .WithMessage("Date range cannot exceed one year.");
        }
    }
}
