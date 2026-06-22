using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorsByCategoryId
{
    public class GetAuthorsByCategoryIdQueryValidator
       : AbstractValidator<GetAuthorsByCategoryIdQuery>
    {
        public GetAuthorsByCategoryIdQueryValidator()
        {
            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("CategoryId is required")
                .GreaterThan(0).WithMessage("CategoryId must be greater than 0");
        }
    }
}
