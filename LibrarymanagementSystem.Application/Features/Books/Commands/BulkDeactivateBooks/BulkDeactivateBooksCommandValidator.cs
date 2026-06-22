using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.BulkDeactivateBooks
{
    public class BulkDeactivateBooksCommandValidator:AbstractValidator<BulkDeactivateBooksCommand>
    {
        public BulkDeactivateBooksCommandValidator()
        {
            RuleFor(x => x.BookIds)
          .NotNull().WithMessage("BookIds list cannot be null.")
          .NotEmpty().WithMessage("BookIds list cannot be empty.");


            RuleForEach(x => x.BookIds)
                .GreaterThan(0).WithMessage("Each BookId must be greater than 0.");


            RuleFor(x => x.BookIds)
                .Must(ids => ids.Distinct().Count() == ids.Count)
                .WithMessage("Duplicate BookIds are not allowed.");

        }
    }
}
