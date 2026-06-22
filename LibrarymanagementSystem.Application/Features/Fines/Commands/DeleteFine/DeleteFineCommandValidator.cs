using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Commands.DeleteFine
{
    public class DeleteFineCommandValidator : AbstractValidator<DeleteFineCommand>
    {
        public DeleteFineCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Fine Id must be greater than 0.");
        }
    }

}
