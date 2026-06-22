using FluentValidation;
using LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorsWithPagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Queries.GetCategoriesWithPagination
{
    public class GetCategoriessWithPaginationQueryValidator : AbstractValidator<GetCategoriesWithPaginationQuery>
    {
        public GetCategoriessWithPaginationQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(100)
                .WithMessage("PageSize must be between 1 and 100");
        }
    }
}
