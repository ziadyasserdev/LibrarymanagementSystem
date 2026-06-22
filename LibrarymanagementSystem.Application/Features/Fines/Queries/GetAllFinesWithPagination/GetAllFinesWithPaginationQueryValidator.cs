using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetAllFinesWithPagination
{
    public class GetAllFinesWithPaginationQueryValidator
    : AbstractValidator<GetAllFinesWithPaginationQuery>
    {
        public GetAllFinesWithPaginationQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than zero.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("Page size must be greater than zero.")
                .LessThanOrEqualTo(50)
                .WithMessage("Page size cannot exceed 50 records.");
        }
    }


}
