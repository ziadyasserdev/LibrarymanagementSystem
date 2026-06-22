using FluentValidation;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetBookByName
{
    public class GetBookByNameQueryValidator : AbstractValidator<GetBookByNameQuery>
    {
        

        public GetBookByNameQueryValidator()
        {
           
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Book title is required.")
                .MaximumLength(200).WithMessage("Book title cannot exceed 200 characters.");

        }
    }
    }
