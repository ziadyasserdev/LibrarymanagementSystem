using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.CheckUserHasReviewed
{
    public class CheckUserHasReviewedQuery:IRequest<Result<bool>>
    {
        public int  Id { get; set; }
        public CheckUserHasReviewedQuery(int id)
        {
            this.Id = id;
        }
    }
}
