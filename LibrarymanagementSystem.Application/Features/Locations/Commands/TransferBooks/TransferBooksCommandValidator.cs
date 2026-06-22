using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Commands.TransferBooks
{
    public class TransferBooksCommandValidator
: AbstractValidator<TransferBooksCommand>
    {
        public TransferBooksCommandValidator()
        {

          

            RuleFor(x => x.FromLocationId)
                .GreaterThan(0);

            RuleFor(x => x.ToLocationId)
                .GreaterThan(0);

         

        }
    }
}
