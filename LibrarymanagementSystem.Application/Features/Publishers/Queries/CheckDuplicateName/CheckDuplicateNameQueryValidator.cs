using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Queries.CheckDuplicateName
{
    public class CheckDuplicateNameQueryValidator : AbstractValidator<CheckDuplicateNameQuery>
    {
        public CheckDuplicateNameQueryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Publisher name is required.")
                .MinimumLength(2)
                .WithMessage("Name must be at least 2 characters long.")
                .MaximumLength(150)
                .WithMessage("Name must not exceed 150 characters.")
                .Must(name => !string.IsNullOrWhiteSpace(name))
                .WithMessage("Name cannot be empty or whitespace.")
                .Must(name => !name.Any(char.IsDigit))
                .WithMessage("Name must not contain numbers.");
        }
    }
}
