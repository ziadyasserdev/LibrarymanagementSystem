using FluentValidation;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LibrarymanagementSystem.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateCategoryCommandValidator(IUnitOfWork unitOfWork)
        {
           
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Category Id must be greater than 0.");


            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Category Name is required.")
                .MaximumLength(100)
                .WithMessage("Category Name cannot exceed 100 characters.");

           
            RuleFor(c=>c)
                .Must(x => IsCategoryNameExistForUpdate(x.Name,x.Id))
                .WithMessage("Category Name already exists. Please choose a different name.");



            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters.");
            this.unitOfWork = unitOfWork;
        }
        private bool IsCategoryNameExistForUpdate(string name,int id)
        {
            return !unitOfWork.Categories.Query().Any(c => c.Name == name && !c.IsDeleted && c.CategoryId != id);
           
        }
    }
}
