using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Queries.GetBookCopiesWithPagination
{
    public class GetBookCopiesWithPaginationQuery:IRequest<Result<PaginatedResult<object>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public GetBookCopiesWithPaginationQuery(int PN, int PS)
        {
            PageNumber = PN;
            PageSize = PS;
        }
    }
}
