using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Commands.RestoreBookCopy
{
    public class RestoreBookCopyCommandValidator
          : AbstractValidator<RestoreBookCopyCommand>
    {
        public RestoreBookCopyCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid BookCopy Id");
        }
    }
}
