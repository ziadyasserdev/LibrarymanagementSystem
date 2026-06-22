using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.BookCopies.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Queries.GetAvailableBookCopies
{
    public class GetAvailableBookCopiesQuery : IRequest<Result<PaginatedResult<UserBookCopyDto>>>
    {
        public int BookId { get; set; }

      
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
