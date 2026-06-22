using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Queries.GetBookCopy
{
    public class GetBookCopyQueryValidator : AbstractValidator<GetBookCopyQuery>
    {
        public GetBookCopyQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("BookCopy Id must be greater than 0.");
        }
    }
}
