using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Commands.EditBranch
{
    public class EditBranchCommandValidator : AbstractValidator<EditBranchCommand>
    {
        public EditBranchCommandValidator()
        {
           
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Branch Id must be greater than 0");

          
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Branch name is required")
                .MaximumLength(100).WithMessage("Branch name cannot exceed 100 characters");

       
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Branch code is required")
                .MaximumLength(10).WithMessage("Branch code cannot exceed 10 characters");

       
            RuleFor(x => x.Address)
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters");

           
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required")
                .MaximumLength(50).WithMessage("City cannot exceed 50 characters");

          
            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?\d{7,15}$").WithMessage("Invalid phone number")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

         
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format")
                .When(x => !string.IsNullOrEmpty(x.Email));
        }
    }
}
