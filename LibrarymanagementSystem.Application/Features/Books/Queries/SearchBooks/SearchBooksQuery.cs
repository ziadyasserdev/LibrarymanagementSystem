using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.SearchBooks
{
    public class SearchBooksQuery:IRequest<Result<PaginatedResult<object>>>
    {
        public string? SearchTerm { get; set; }

        public int? AuthorId { get; set; }
        public int? CategoryId { get; set; }
        public int? PublisherId { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public BookStatusFilter Status { get; set; } = BookStatusFilter.All;

        public BookSortBy SortBy { get; set; } = BookSortBy.Title;
        public bool Desc { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
