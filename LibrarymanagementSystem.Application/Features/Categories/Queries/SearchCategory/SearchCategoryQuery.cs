using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Queries.SearchCategory
{
    public class SearchCategoryQuery:IRequest<Result<PaginatedResult<object>>>
    {
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public CategorySort categorySort { get; set; } = CategorySort.ByName;
        public bool Descending { get; set; } = false;
        public bool? IncludeDeleted { get; set; } = false;
    }
}
