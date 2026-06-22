using FluentValidation;
using LibrarymanagementSystem.Application.Features.Publishers.Commands.CreatePublisher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Commands.BulkCreatePublishers
{
    public class BulkCreatePublishersCommandValidator
       : AbstractValidator<BulkCreatePublishersCommand>
    {
        public BulkCreatePublishersCommandValidator()
        {
            RuleFor(x => x.Publishers)
                .NotNull().WithMessage("Publishers list cannot be null.")
                .NotEmpty().WithMessage("Publishers list cannot be empty.")
                .Must(p => p.Count <= 50)
                .WithMessage("Maximum 50 publishers allowed per request.");

          
           
        
            RuleFor(x => x.Publishers)
                .Must(NoDuplicateNames)
                .WithMessage("Duplicate publisher names found in request.");
        }

        private bool NoDuplicateNames(List<CreatePublisherDto> publishers)
        {
            return publishers
                .Select(p => p.Name.Trim().ToLower())
                .Distinct()
                .Count() == publishers.Count;
        }
    }


    
}
