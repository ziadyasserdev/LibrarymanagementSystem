using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Commands.BulkDeletePublishers
{
    public class BulkDeletePublishersValidator
  : AbstractValidator<BulkDeletePublishersCommand>
    {
        public BulkDeletePublishersValidator()
        {

            RuleFor(x => x.PublisherIds)
                .NotEmpty()
                .WithMessage("PublisherIds required");

        }
    }
}
