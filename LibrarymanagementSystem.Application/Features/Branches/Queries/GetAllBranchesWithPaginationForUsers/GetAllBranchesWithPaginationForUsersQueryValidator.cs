using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Queries.GetAllBranchesWithPaginationForUsers
{
    public class GetAllBranchesWithPaginationForUsersQueryValidator
         : AbstractValidator<GetAllBranchesWithPaginationForUsersQuery>
    {
        public GetAllBranchesWithPaginationForUsersQueryValidator()
        {
            
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than 0")
                .LessThanOrEqualTo(50)
                .WithMessage("PageSize cannot exceed 50");

           
            RuleFor(x => x.Search)
                .MaximumLength(100)
                .WithMessage("Search term cannot exceed 100 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.Search));

            RuleFor(x => x.City)
                .MaximumLength(50)
                .WithMessage("City cannot exceed 50 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.City));

           
            RuleFor(x => x.BranchFilter)
                .IsInEnum()
                .WithMessage("Invalid BranchFilter value");

            RuleFor(x => x.BranchSort)
                .IsInEnum()
                .WithMessage("Invalid BranchSort value");
        }
    }
}
