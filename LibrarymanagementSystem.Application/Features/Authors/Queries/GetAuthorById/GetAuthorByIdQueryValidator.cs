using FluentValidation;
using LibrarymanagementSystem.Application.Features.Authors.Commands.DeleteAuthor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorById
{
    public class GetAuthorByIdQueryValidator : AbstractValidator<GetAuthorByIdQuery>
    {
        public GetAuthorByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Author ID is required")
                .NotNull().WithMessage("Author ID cannot be null")
                .GreaterThan(0).WithMessage("Author ID must be greater than 0");

        }
    }
}
