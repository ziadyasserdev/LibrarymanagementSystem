using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.BookCopies.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Queries.GetBookCopiesStatistics
{
    public class GetBookCopiesStatisticsQuery : IRequest<Result<BookCopyStatisticsDto>>
    {
        
        public int? BookId { get; set; }
        public int? BranchId { get; set; }
        public int? LocationId { get; set; }
    }
}
