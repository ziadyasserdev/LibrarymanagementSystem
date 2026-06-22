using LibrarymanagementSystem.Application.Common.Caching;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksByCategoryName
{
    public class GetBooksByCategoryNameQuery: IRequest<Result<List<BookDto>>>,ICacheable
    {
        public string CategoryName { get; set; }

        public string CacheKey => CacheKeys.Books.ByCategoryName(CategoryName);

        public TimeSpan CacheDuration => TimeSpan.FromMinutes(1);

        public GetBooksByCategoryNameQuery(string CategoryName)
        {
            this.CategoryName = CategoryName;
        }
    }
}
