using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Queries.GetCategoriesByAuthorId
{
    public class GetCategoriesByAuthorIdQueryValidator
        : AbstractValidator<GetCategoriesByAuthorIdQuery>
    {
        public GetCategoriesByAuthorIdQueryValidator()
        {
            RuleFor(x => x.AuthorId)
                .NotEmpty()
                .WithMessage("AuthorId is required")

                .GreaterThan(0)
                .WithMessage("AuthorId must be greater than 0");
        }
    }
}
