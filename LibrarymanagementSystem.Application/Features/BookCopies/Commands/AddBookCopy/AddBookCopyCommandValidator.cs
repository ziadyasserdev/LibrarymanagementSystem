using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Commands.AddBookCopy
{
    public class AddBookCopyCommandValidator : AbstractValidator<AddBookCopyCommand>
    {
        public AddBookCopyCommandValidator()
        {
            RuleFor(x => x.Barcode)
                .NotEmpty()
                .WithMessage("Barcode is required.")
                .MinimumLength(3)
                .WithMessage("Barcode must be at least 3 characters long.");

            RuleFor(x => x.BookId)
                .GreaterThan(0)
                .WithMessage("BookId must be greater than 0.");

            RuleFor(x => x.LocationId)
                .GreaterThan(0)
                .WithMessage("LocationId must be greater than 0.");
        }
    }
}
