using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Commands.BulkRestoreBookCopies
{
    public class BulkRestoreBookCopiesCommandValidator
      : AbstractValidator<BulkRestoreBookCopiesCommand>
    {
        public BulkRestoreBookCopiesCommandValidator()
        {
            RuleFor(x => x.Ids)
                .NotEmpty().WithMessage("Ids list cannot be empty");

            RuleForEach(x => x.Ids)
                .GreaterThan(0);
        }
    }
}
