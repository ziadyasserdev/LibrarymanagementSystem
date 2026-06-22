using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetAllFinesByUserId
{
    public class GetAllFinesByUserIdQueryValidator
        : AbstractValidator<GetAllFinesByUserIdQuery>
    {
        public GetAllFinesByUserIdQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.")
                .MaximumLength(50) 
                .WithMessage("UserId must not exceed 50 characters.");

           
            RuleFor(x => x.UserId)
                .Must(id => Guid.TryParse(id, out _))
                .WithMessage("UserId must be a valid GUID.");
        }
    }
}
