using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.DeactivateBook
{
    public class DeactivateBookCommandValidator:AbstractValidator<DeactivateBookCommand>
    {
        public DeactivateBookCommandValidator()
        {
            RuleFor(x => x.Id)
              .GreaterThan(0)
              .WithMessage("Book Id must be greater than zero.");
        }
    }
}
