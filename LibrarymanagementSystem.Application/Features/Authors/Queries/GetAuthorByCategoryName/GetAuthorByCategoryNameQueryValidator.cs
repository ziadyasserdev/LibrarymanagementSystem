using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorByCategoryName
{
    public class GetAuthorByCategoryNameQueryValidator
         : AbstractValidator<GetAuthorByCategoryNameQuery>
    {
        public GetAuthorByCategoryNameQueryValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category name is required")
                .NotNull().WithMessage("Category name cannot be null")
                .MinimumLength(2).WithMessage("Category name must be at least 2 characters")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters");
        }
    }
}
