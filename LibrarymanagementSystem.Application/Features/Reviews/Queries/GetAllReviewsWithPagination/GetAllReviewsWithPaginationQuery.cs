using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Reviews.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.GetAllReviewsWithPagination
{
    public class GetAllReviewsWithPaginationQuery:IRequest<Result<PaginatedResult<AdminReviewDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        // Filters
        public int? BookId { get; set; }
        public string? UserId { get; set; }
        public int? Rating { get; set; }
        public bool? IsDeleted { get; set; }

        // Sorting
        public string? OrderBy { get; set; } = "CreatedAt"; // CreatedAt, Rating
        public bool OrderByDescending { get; set; } = true;
    }
}
