using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetFinePaymentSummary
{
    public class GetFinePaymentSummaryQueryValidator:AbstractValidator<GetFinePaymentSummaryQuery>
    {
        public GetFinePaymentSummaryQueryValidator()
        {
            RuleFor(x => x.FineId)
               .NotEmpty().WithMessage("FineId is required.")
               .GreaterThan(0).WithMessage("FineId must be greater than 0.");
        }
    }
}
