using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Authors.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorsWithPagination
{
    public class GetAuthorsWithPaginationQuery : IRequest<Result<PaginatedResult<AuthorDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public GetAuthorsWithPaginationQuery(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }
    }
    
}
