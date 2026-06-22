using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.CanDeleteAuthor
{
    public class CanDeleteAuthorQueryValidator : AbstractValidator<CanDeleteAuthorQuery>
    {
        public CanDeleteAuthorQueryValidator()
        {
            RuleFor(x => x.AuthorId)
                .GreaterThan(0).WithMessage("AuthorId must be greater than 0.");
        }
    }
}
