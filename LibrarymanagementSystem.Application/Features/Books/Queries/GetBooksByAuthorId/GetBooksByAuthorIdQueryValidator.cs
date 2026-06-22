using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksByAuthorId
{
    public class GetBooksByAuthorIdQueryValidator
         : AbstractValidator<GetBooksByAuthorIdQuery>
    {
        public GetBooksByAuthorIdQueryValidator()
        {
            RuleFor(x => x.AuthorId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("AuthorId is required.")
                .GreaterThan(0).WithMessage("AuthorId must be greater than 0.");
        }
    }
}
