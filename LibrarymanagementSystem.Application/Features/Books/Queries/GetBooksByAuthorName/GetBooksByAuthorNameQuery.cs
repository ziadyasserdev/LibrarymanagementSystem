using LibrarymanagementSystem.Application.Common.Caching;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksByAuthorName
{
    public class GetBooksByAuthorNameQuery:IRequest<Result<List<BookDto>>>,ICacheable
    {
        public string AuthorName { get; set; }

        public string CacheKey => CacheKeys.Books.ByAuthorName(AuthorName);

        public TimeSpan CacheDuration => TimeSpan.FromMinutes(1);

        public GetBooksByAuthorNameQuery(string AuthorName)
        {
            this.AuthorName = AuthorName;
        }
    }
}
