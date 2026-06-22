using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.LoanBooks.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetLostBooksWithPagination
{
    public  class GetLostBooksWithPaginationQuery:IRequest<Result<PaginatedResult<LostBookDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public GetLostBooksWithPaginationQuery(int pn , int ps)
        {
            PageNumber = pn;
            PageSize = ps;
        }
    }
}
