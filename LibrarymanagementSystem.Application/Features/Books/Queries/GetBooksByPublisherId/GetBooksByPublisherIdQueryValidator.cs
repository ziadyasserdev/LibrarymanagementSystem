using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksByPublisherId
{
    public class GetBooksByPublisherIdQueryValidator
     : AbstractValidator<GetBooksByPublisherIdQuery>
    {
        public GetBooksByPublisherIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Publisher Id must be greater than 0.");
        }
    }


}
