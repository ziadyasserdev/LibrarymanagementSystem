using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksWithPagination
{
    public class GetBooksWithPaginationQuery:IRequest<Result<PaginatedResult<BookDto>>>
    {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public GetBooksWithPaginationQuery(int pageNumber,int pageSize)
            {
                this.PageNumber = pageNumber;
                this.PageSize = pageSize;
        }
    }
}
