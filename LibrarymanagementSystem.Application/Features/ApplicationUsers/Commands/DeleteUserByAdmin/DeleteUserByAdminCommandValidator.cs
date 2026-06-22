using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.ApplicationUsers.Commands.DeleteUserByAdmin
{
    public class DeleteUserByAdminCommandValidator
        : AbstractValidator<DeleteUserByAdminCommand>
    {
        public DeleteUserByAdminCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.")
                .NotNull()
                .WithMessage("UserId cannot be null.")
                .MaximumLength(450)
                .WithMessage("UserId is too long.");
        }
    }
}
