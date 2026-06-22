using FluentValidation;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.CreateBook
{
    public class CreateBookCommandValidator
         : AbstractValidator<CreateBookCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public CreateBookCommandValidator(IUnitOfWork unitOfWork)
        {

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(x => x)
                .Must(CheckTitleExist)
                 .WithMessage("A book with the same title already exists.");
           




            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");


            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage("ISBN is required.")
                .Length(10, 13).WithMessage("ISBN must be 10 or 13 characters.");

            RuleFor(p => p)
     .Must(CheckISBNExist)
     .WithMessage("A book with the same ISBN already exists.");

            RuleFor(x => x.PublishedYear)
                .InclusiveBetween(1500, DateTime.Now.Year)
                .WithMessage($"Published year must be between 1500 and {DateTime.Now.Year}.");

           
            RuleFor(x => x.NumberOfPages)
                .GreaterThan(0)
                .WithMessage("Number of pages must be greater than zero.");

           
            RuleFor(x => x.Language)
                .NotEmpty().WithMessage("Language is required.")
                .MaximumLength(50);

           
           

        
            RuleFor(x => x.Edition)
                .NotEmpty().WithMessage("Edition is required.")
                .MaximumLength(50);




            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than zero.");

           
           
  
            RuleFor(x => x.AuthorId)
                .GreaterThan(0)
                .WithMessage("AuthorId must be greater than zero.");
  
            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("CategoryId must be greater than zero.");
            this.unitOfWork = unitOfWork;
        }

        private bool CheckTitleExist(CreateBookCommand command)
        {
            return !unitOfWork.Books.Query()
         .Any(c => c.Title.Trim().ToLower() == command.Title.Trim().ToLower()
                   && !c.IsDeleted
                   && c.AuthorId == command.AuthorId);
        }
        private bool CheckISBNExist(CreateBookCommand command)
        {
            return !unitOfWork.Books.Query()
         .Any(c => c.ISBN == command.ISBN
                   && !c.IsDeleted
                   && c.AuthorId == command.AuthorId);
        }
    }
}


