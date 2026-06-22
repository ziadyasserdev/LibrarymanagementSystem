using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.CanDeleteBook
{
    public class CanDeleteBookQueryValidator : AbstractValidator<CanDeleteBookQuery>
    {
        public CanDeleteBookQueryValidator()
        {
            RuleFor(x => x.BookId)
                .GreaterThan(0)
                .WithMessage("BookId must be greater than 0.");
        }
    }
}
