using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetAllUnpaidFines
{
    public class GetAllUnpaidFinesQueryValidator
    : AbstractValidator<GetAllUnpaidFinesQuery>
    {
        public GetAllUnpaidFinesQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(50)
                .WithMessage("PageSize must not exceed 50.");
        }
    }
}
