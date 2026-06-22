using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.ApplicationUsers.Queries.GetAllUsersByStatus
{
    public class GetAllUsersByStatusQueryValidator : AbstractValidator<GetAllUsersByStatusQuery>
    {
        public GetAllUsersByStatusQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(100)
                .WithMessage("PageSize cannot be more than 100.");

            RuleFor(x => x.UserStatus)
                .IsInEnum()
                .WithMessage("Invalid user status.");
        }
    }
}
