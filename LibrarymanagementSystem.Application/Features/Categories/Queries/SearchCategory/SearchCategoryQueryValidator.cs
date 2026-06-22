using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Queries.SearchCategory
{
    public class SearchCategoryQueryValidator : AbstractValidator<SearchCategoryQuery>
    {
        public SearchCategoryQueryValidator()
        {
         
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0.");

                 RuleFor(x => x.PageSize)   
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(100)
                .WithMessage("PageSize cannot exceed 100.");

        
            RuleFor(x => x.categorySort)
                .IsInEnum()
                .WithMessage("CategorySort must be a valid sorting option.");

           
            RuleFor(x => x.SearchTerm)
                .MinimumLength(2)
                .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm))
                .WithMessage("SearchTerm must be at least 2 characters long.");
        }
    }
}
