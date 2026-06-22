using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Reviews.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.GetMyReviewsWithPagination
{
    public class GetMyReviewsWithPaginationQuery:IRequest<Result<PaginatedResult<MyReviewDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
       public ReviewFilter? ReviewFilter { get; set; }
        public bool? OrderByDescending { get; set; } = true;
        public GetMyReviewsWithPaginationQuery(int pn, int ps, ReviewFilter? ReviewFilter, bool? OrderByDescending)
        {
           PageNumber=pn;
           PageSize=ps;    
           this.ReviewFilter=ReviewFilter;
           this.OrderByDescending=OrderByDescending;
        }
    }
}
