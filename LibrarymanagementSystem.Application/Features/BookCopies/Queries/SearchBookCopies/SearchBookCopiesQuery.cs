using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Queries.SearchBookCopies
{
    public class SearchBookCopiesQuery:IRequest<Result<PaginatedResult<object>>>
    {
        public string? SearchTerm { get; set; } 
        public int? BookId { get; set; }
        public int? LocationId { get; set; }
        public int? BranchId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
