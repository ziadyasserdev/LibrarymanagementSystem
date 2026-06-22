using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Commands.DeleteBookCopy
{
    public class DeleteBookCopyCommandValidator : AbstractValidator<DeleteBookCopyCommand>
    {
        public DeleteBookCopyCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Book copy Id must be greater than 0.");
        }
    }
}
