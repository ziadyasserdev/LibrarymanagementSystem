using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Queries.GetPublisherStatistics
{
    public class GetPublisherStatisticsValidator
      : AbstractValidator<GetPublisherStatisticsQuery>
    {
        public GetPublisherStatisticsValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Publisher Id must be greater than 0.");
        }
    }
}
