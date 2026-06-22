using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Commands.AddFine
{
    public class AddFineCommandValidator : AbstractValidator<AddFineCommand>
    {
        public AddFineCommandValidator()
        {
            RuleFor(x => x.LoanBookId)
                .GreaterThan(0).WithMessage("LoanBookId must be greater than 0.");

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0).WithMessage("TotalAmount must be greater than 0.");

            RuleFor(x => x.FineType)
                .IsInEnum().WithMessage("Invalid FineType.");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");
        }
    }
}
