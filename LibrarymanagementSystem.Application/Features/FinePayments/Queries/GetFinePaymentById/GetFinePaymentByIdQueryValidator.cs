using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetFinePaymentById
{
    public class GetFinePaymentByIdQueryValidator
       : AbstractValidator<GetFinePaymentByIdQuery>
    {
        public GetFinePaymentByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Fine Payment Id must be greater than 0.");
        }
    }
}
