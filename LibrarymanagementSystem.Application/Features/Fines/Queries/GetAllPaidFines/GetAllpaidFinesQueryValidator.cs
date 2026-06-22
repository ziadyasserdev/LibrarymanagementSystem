using FluentValidation;
using LibrarymanagementSystem.Application.Features.Fines.Queries.GetAllUnpaidFines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetAllPaidFines
{
    public class GetAllpaidFinesQueryValidator
   : AbstractValidator<GetAllUnpaidFinesQuery>
    {
        public GetAllpaidFinesQueryValidator()
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
