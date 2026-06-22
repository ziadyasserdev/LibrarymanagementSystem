using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Queries.GetCategoriesByAuthorName
{
    public class GetCategoriesByAuthorNameQueryValidator
      : AbstractValidator<GetCategoriesByAuthorNameQuery>
    {
        public GetCategoriesByAuthorNameQueryValidator()
        {
            RuleFor(x => x.AuthorName)
                .NotEmpty()
                .WithMessage("AuthorName is required")

                .MinimumLength(2)
                .WithMessage("AuthorName must be at least 2 characters")

                .MaximumLength(100)
                .WithMessage("AuthorName must not exceed 100 characters");
        }
    }
}
