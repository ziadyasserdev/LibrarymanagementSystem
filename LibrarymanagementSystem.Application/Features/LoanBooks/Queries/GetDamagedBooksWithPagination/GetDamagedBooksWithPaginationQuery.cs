using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.LoanBooks.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetDamagedBooksWithPagination
{
    public class GetDamagedBooksWithPaginationQuery:IRequest<Result<PaginatedResult<DamageBookDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public GetDamagedBooksWithPaginationQuery(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }
    }
}
