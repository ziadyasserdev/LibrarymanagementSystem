using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksByCategoryName
{
    public class GetBooksByCategoryNameQueryValidator
        : AbstractValidator<GetBooksByCategoryNameQuery>
    {
        public GetBooksByCategoryNameQueryValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotNull().WithMessage("Category name is required.")
                .NotEmpty().WithMessage("Category name cannot be empty.")
                .MinimumLength(2).WithMessage("Category name must be at least 2 characters.")
                .MaximumLength(100).WithMessage("Category name is too long.");
        }
    }
}
