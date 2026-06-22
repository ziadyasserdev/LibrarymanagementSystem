using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Commands.BulkRestoreAuthor
{
    public class BulkRestoreAuthorCommandValidator:AbstractValidator<BulkRestoreAuthorCommand>
    {
        public BulkRestoreAuthorCommandValidator()
        {
            RuleFor(x => x.AuthorIds)
             .NotNull().WithMessage("AuthorIds list cannot be null.")
             .NotEmpty().WithMessage("AuthorIds list cannot be empty.");

            RuleForEach(x => x.AuthorIds)
                .GreaterThan(0).WithMessage("Each AuthorId must be greater than 0.");

            RuleFor(x => x.AuthorIds)
                .Must(ids => ids.Distinct().Count() == ids.Count)
                .WithMessage("Duplicate AuthorIds are not allowed.");
        }
    }
}
