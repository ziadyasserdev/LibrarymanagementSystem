using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorByName
{
    public class GetAuthorByNameQueryValidator
        : AbstractValidator<GetAuthorByNameQuery>
    {
        public GetAuthorByNameQueryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Author name is required.")
                .MinimumLength(2).WithMessage("Author name must be at least 2 characters.")
                .MaximumLength(100).WithMessage("Author name must not exceed 100 characters.");
        }
    }
}
