using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Queries.GetAllBranchesWithPaginationForAdmin
{
    public class GetAllBranchesWithPaginationForAdminQueryValidator
        : AbstractValidator<GetAllBranchesWithPaginationForAdminQuery>
    {
        public GetAllBranchesWithPaginationForAdminQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("PageNumber must be greater than 0");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0")
                .LessThanOrEqualTo(100).WithMessage("PageSize cannot exceed 100");

            RuleFor(x => x.BranchStatus)
                .IsInEnum().WithMessage("Invalid BranchStatus value");
        }
    }
}
