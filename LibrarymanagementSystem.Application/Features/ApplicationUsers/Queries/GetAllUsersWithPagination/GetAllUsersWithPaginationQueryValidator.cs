using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.ApplicationUsers.Queries.GetAllUsersWithPagination
{
    public class GetAllUsersWithPaginationQueryValidator
     : AbstractValidator<GetAllUsersWithPaginationQuery>
    {
        public GetAllUsersWithPaginationQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageNumber must be greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageSize must be greater than or equal to 1.")
                .LessThanOrEqualTo(100)
                .WithMessage("PageSize must not exceed 100.");
        }
    }
    }
