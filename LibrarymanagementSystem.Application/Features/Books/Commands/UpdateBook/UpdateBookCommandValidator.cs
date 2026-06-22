using FluentValidation;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Books.Commands.CreateBook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.UpdateBook
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateBookCommandValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Book Id is required and must be greater than zero.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            RuleFor(v => v)
      .Must(CheckISBNExist)
      .WithMessage("ISBN already exists.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage("ISBN is required.")
                .Length(10, 13).WithMessage("ISBN must be 10 or 13 characters.");

            RuleFor(u => u)
                .Must(CheckTitleExist)
                .WithMessage("A book with the same title already exists for this author.");

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
          

        }

        private bool CheckTitleExist(UpdateBookCommand command)
        {
            return !unitOfWork.Books.Query()
        .Any(c => c.Title.Trim().ToLower() == command.Title.Trim().ToLower()
                  && !c.IsDeleted
                  && c.AuthorId == command.AuthorId
                  && c.Id != command.Id);
        }
        private bool CheckISBNExist(UpdateBookCommand command)
        {
            return !unitOfWork.Books.Query()
         .Any(c => c.ISBN == command.ISBN
                   && !c.IsDeleted
                   && c.AuthorId == command.AuthorId
                   && c.Id != command.Id);
        }
    }
}