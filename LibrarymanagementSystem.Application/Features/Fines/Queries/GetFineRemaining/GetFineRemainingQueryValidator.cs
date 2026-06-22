using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetFineRemaining
{
    public class GetFineRemainingQueryValidator : AbstractValidator<GetFineRemainingQuery>
    {
        public GetFineRemainingQueryValidator()
        {
            RuleFor(x => x.FineId)
                .GreaterThan(0)
                .WithMessage("FineId must be greater than 0.");
        }
    }
}
