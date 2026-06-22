using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Queries.GetBookCopiesStatistics
{
    public class GetBookCopiesStatisticsQueryValidator : AbstractValidator<GetBookCopiesStatisticsQuery>
    {
        public GetBookCopiesStatisticsQueryValidator()
        {
            RuleFor(x => x.BookId)
                .GreaterThan(0)
                .When(x => x.BookId.HasValue)
                .WithMessage("BookId must be greater than 0.");

            RuleFor(x => x.BranchId)
                .GreaterThan(0)
                .When(x => x.BranchId.HasValue)
                .WithMessage("BranchId must be greater than 0.");

            RuleFor(x => x.LocationId)
                .GreaterThan(0)
                .When(x => x.LocationId.HasValue)
                .WithMessage("LocationId must be greater than 0.");
        }
    }
}
