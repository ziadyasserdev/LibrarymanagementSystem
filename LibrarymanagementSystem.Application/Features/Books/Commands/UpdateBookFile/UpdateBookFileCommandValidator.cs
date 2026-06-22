using FluentValidation;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Books.Commands.CreateBook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.UpdateBookFile
{
    public class UpdateBookFileCommandValidator
      : AbstractValidator<UpdateBookFileCommand>
    {
        public UpdateBookFileCommandValidator()
        {
            RuleFor(x => x.BookId)
                .GreaterThan(0)
                .WithMessage("BookId must be greater than zero.");

            RuleFor(x => x.NewFile)
                .NotNull()
                .WithMessage("File is required.");

            RuleFor(x => x.NewFile.Length)
                .GreaterThan(0)
                .When(x => x.NewFile != null)
                .WithMessage("File cannot be empty.");


        }
      
    }
    }
