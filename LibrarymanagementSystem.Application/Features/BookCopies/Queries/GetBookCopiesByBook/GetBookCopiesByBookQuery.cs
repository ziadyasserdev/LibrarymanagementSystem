using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.BookCopies.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Queries.GetBookCopiesByBook
{
    public class GetBookCopiesByBookQuery:IRequest<Result<PaginatedResult<BookCopyAdminDto>>>
    {
        public int BookId { get; set; }

      
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        
        public BookCopyStatus? Status { get; set; }
    
        public int? LocationId { get; set; }
        public int? BranchId { get; set; }

     public bool? IncludeDeleted { get; set; } = false;
        public string? SortBy { get; set; } = "Id"; 
        public bool SortDescending { get; set; } = false;
    }
}
