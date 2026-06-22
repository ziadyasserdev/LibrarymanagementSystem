using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.GetSpecificLocationStatistics
{
    public class GetSpecificLocationStatisticsQueryValidator
       : AbstractValidator<GetSpecificLocationStatisticsQuery>
    {
        public GetSpecificLocationStatisticsQueryValidator()
        {
            RuleFor(x => x.LocationId)
                .GreaterThan(0)
                .WithMessage("Location Id must be greater than 0.");
        }
    }
}
