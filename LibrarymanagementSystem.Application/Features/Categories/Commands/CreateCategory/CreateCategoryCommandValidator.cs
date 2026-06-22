using FluentValidation;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator
        : AbstractValidator<CreateCategoryCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public CreateCategoryCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MinimumLength(2).WithMessage("Category name must be at least 2 characters.")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.")
                .Must(IsCategoryNameExist).WithMessage("Category name already exists.");
            
            RuleFor(x => x.Description)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.Description));
            this.unitOfWork = unitOfWork;
        }

        private bool IsCategoryNameExist(string name)
        {
          
                return !unitOfWork.Categories.Query().Any(c => c.Name == name && !c.IsDeleted);
        }
    }

   
   
}
