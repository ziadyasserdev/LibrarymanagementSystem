using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.SearchAuthors
{
    public class SearchAuthorsQuery:IRequest<Result<PaginatedResult<object>>>
    {
        public string? SearchTerm { get; set; }        
        public int PageNumber { get; set; } = 1;       
        public int PageSize { get; set; } = 10;       
        public AuthorSort AuthorSort { get; set; } = AuthorSort.Name;
        public bool Descending { get; set; } = false;  
        public bool? IncludeDeleted { get; set; } = false;  
    }
}
