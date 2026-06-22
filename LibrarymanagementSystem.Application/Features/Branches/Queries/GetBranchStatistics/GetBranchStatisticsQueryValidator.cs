using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Queries.GetBranchStatistics
{
    public class GetBranchStatisticsQueryValidator : AbstractValidator<GetBranchStatisticsQuery>
    {
        public GetBranchStatisticsQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Branch Id must be greater than 0");
        }
    }
}
