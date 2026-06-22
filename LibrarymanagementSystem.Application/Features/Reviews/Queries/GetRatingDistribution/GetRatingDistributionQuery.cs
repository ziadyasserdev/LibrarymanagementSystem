using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.GetRatingDistribution
{
    public class GetRatingDistributionQuery:IRequest<Result<Dictionary<int,int>>>
    {
        public int Id { get; set; }
        public GetRatingDistributionQuery(int id)
        {
            this.Id = id;
        }
    }
}
