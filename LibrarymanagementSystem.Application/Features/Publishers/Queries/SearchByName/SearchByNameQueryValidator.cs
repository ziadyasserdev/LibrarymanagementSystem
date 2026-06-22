using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Queries.SearchByName
{
    public class SearchByNameQueryValidator : AbstractValidator<SearchByNameQuery>
    {
        public SearchByNameQueryValidator()
        {
          
            RuleFor(x => x.name)
                
                .MaximumLength(150).WithMessage("Publisher name must be at most 150 characters.");

           
            RuleFor(x => x.country)
                .MaximumLength(100).WithMessage("Country must be at most 100 characters.");

            RuleFor(x => x.publisherStatusFilter)
                .IsInEnum().WithMessage("Invalid publisher status filter.");
        }
    }
}
