using FluentValidation;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetBookById
{
    public class GetBookByIdQueryValidator : AbstractValidator<GetBookByIdQuery>
    {
       

        public GetBookByIdQueryValidator()
        {
           

            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Book Id must be greater than zero.");

           
        }
    }
    }
