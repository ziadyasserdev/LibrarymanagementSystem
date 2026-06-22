using FluentValidation;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateAuthorCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(a => a.AuthorId)
                .GreaterThan(0).WithMessage("AuthorId must be greater than 0.");

            RuleFor(a => a.Name)
      .NotEmpty().WithMessage("Name is required.")
      .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.")
     ;
    
            RuleFor(a => a.Biography)
                .MaximumLength(1000).WithMessage("Biography cannot exceed 1000 characters.");

            RuleFor(a => a.DateOfBirth)
                .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("Date of Birth cannot be in the future.");
            this.unitOfWork = unitOfWork;
        }

        
    }
}
