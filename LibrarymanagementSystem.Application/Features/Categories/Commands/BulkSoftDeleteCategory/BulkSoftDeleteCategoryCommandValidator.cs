using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Commands.BulkSoftDeleteCategory
{
    public class BulkSoftDeleteCategoryCommandValidator:AbstractValidator<BulkSoftDeleteCategoryCommand>
    {
        public BulkSoftDeleteCategoryCommandValidator()
        {
            RuleFor(x => x.CategoryIds)
             .NotNull()
             .WithMessage("CategoryIds list cannot be null.")
             .NotEmpty()
             .WithMessage("CategoryIds list cannot be empty.");

            RuleForEach(x => x.CategoryIds)
                .GreaterThan(0)
                .WithMessage("Category Id must be greater than 0.");

            RuleFor(x => x.CategoryIds)
                .Must(ids => ids.Distinct().Count() == ids.Count)
                .WithMessage("Duplicate Category Ids are not allowed.");
        }
    }
}
