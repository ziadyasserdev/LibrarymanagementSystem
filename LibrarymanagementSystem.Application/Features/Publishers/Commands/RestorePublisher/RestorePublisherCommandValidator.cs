using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Commands.RestorePublisher
{
    public class RestorePublisherCommandValidator : AbstractValidator<RestorePublisherCommand>
    {
        public RestorePublisherCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Publisher Id is required")
                .GreaterThan(0).WithMessage("Publisher Id must be greater than 0");
        }
    }
}
