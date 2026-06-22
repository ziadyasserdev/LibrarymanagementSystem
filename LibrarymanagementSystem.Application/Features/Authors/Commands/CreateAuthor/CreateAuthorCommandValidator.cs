using FluentValidation;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Commands.CreateAuthor
{
    public class CreateAuthorCommandValidator
       : AbstractValidator<CreateAuthorCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateAuthorCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Author name is required")
                .MinimumLength(3).WithMessage("Author name must be at least 3 characters")
                .MaximumLength(100).WithMessage("Author name must not exceed 100 characters")
              ;

            RuleFor(x => x.Biography)
                .MaximumLength(1000)
                .WithMessage("Biography must not exceed 1000 characters");

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.Today)
                .When(x => x.DateOfBirth.HasValue)
                .WithMessage("Date of birth must be in the past");
        }

        
    }
}
