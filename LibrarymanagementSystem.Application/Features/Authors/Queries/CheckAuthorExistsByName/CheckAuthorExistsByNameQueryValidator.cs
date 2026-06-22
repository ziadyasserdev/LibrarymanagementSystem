using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.CheckAuthorExistsByName
{
    public class CheckAuthorExistsByNameQueryValidator : AbstractValidator<CheckAuthorExistsByNameQuery>
    {
        public CheckAuthorExistsByNameQueryValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Author name cannot be null.")
                .NotEmpty().WithMessage("Author name cannot be empty.")
                .Must(name => !string.IsNullOrWhiteSpace(name))
                .WithMessage("Author name cannot be whitespace only.")
                .MaximumLength(200).WithMessage("Author name cannot exceed 200 characters."); // Optional
        }
    }

}
