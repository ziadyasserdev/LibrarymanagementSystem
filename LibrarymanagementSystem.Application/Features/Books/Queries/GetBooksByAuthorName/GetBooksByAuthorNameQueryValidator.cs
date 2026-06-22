using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksByAuthorName
{
    public class GetBooksByAuthorNameQueryValidator
        : AbstractValidator<GetBooksByAuthorNameQuery>
    {
        public GetBooksByAuthorNameQueryValidator()
        {
            RuleFor(x => x.AuthorName)
                .NotNull().WithMessage("Author name is required.")
                .NotEmpty().WithMessage("Author name cannot be empty.")
                .MinimumLength(2).WithMessage("Author name must be at least 2 characters.")
                .MaximumLength(100).WithMessage("Author name is too long.");
        }
    }
}
