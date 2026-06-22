using FluentValidation;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Commands.EditBookCopy
{
    public class EditBookCopyCommandValidator : AbstractValidator<EditBookCopyCommand>
    {
        public EditBookCopyCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("BookCopy Id must be greater than 0.");

            RuleFor(x => x.Barcode)
                .NotEmpty().WithMessage("Barcode is required.")
                .MaximumLength(50).WithMessage("Barcode must not exceed 50 characters.");

            RuleFor(x => x.BookId)
                .GreaterThan(0).WithMessage("BookId must be greater than 0.");

            RuleFor(x => x.LocationId)
                .GreaterThan(0).WithMessage("LocationId must be greater than 0.");

          
        }
    }
}
