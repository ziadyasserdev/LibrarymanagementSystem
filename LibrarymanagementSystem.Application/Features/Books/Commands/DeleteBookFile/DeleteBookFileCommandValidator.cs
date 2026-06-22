using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.DeleteBookFile
{
    public class DeleteBookFileCommandValidator : AbstractValidator<DeleteBookFileCommand>
    {
        public DeleteBookFileCommandValidator()
        {
            RuleFor(x => x.BookId)
                .GreaterThan(0)
                .WithMessage("BookId must be greater than 0.");
        }
    }
}
