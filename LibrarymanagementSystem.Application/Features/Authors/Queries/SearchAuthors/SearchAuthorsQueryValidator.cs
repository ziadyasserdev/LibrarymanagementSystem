using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.SearchAuthors
{
    public class SearchAuthorsQueryValidator : AbstractValidator<SearchAuthorsQuery>
    {
        public SearchAuthorsQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(100)
                .WithMessage("PageSize cannot exceed 100.");

            RuleFor(x => x.AuthorSort)
                .IsInEnum()
                .WithMessage("AuthorSort must be a valid sorting option.");
        }
    }
}
