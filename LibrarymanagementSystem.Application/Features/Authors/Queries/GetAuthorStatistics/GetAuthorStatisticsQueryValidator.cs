using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorStatistics
{
    public class GetAuthorStatisticsQueryValidator : AbstractValidator<GetAuthorStatisticsQuery>
    {
        public GetAuthorStatisticsQueryValidator()
        {
            RuleFor(x => x.AuthorId)
                .GreaterThan(0)
                .WithMessage("AuthorId must be greater than zero");
        }
    }
}
