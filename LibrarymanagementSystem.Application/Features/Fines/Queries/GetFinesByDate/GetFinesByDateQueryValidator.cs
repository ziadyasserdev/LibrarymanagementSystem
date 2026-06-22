using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetFinesByDate
{
    public class GetFinesByDateQueryValidator : AbstractValidator<GetFinesByDateQuery>
    {
        public GetFinesByDateQueryValidator()
        {
            RuleFor(x => x.From)
                .NotEmpty()
                .WithMessage("From date is required");

            RuleFor(x => x.To)
                .NotEmpty()
                .WithMessage("To date is required");

            RuleFor(x => x)
                .Must(x => x.From <= x.To)
                .WithMessage("From date must be less than or equal to To date");

            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(50)
                .WithMessage("PageSize must be between 1 and 50");
        }
    }
}
