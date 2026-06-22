using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetFineById
{

    public class GetFineByIdQueryValidator
       : AbstractValidator<GetFineByIdQuery>
    {
        public GetFineByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Fine Id must be greater than zero.");
        }
    }
}
