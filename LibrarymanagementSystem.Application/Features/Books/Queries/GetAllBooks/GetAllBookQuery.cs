using LibrarymanagementSystem.Application.Common.Caching;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetAllBooks
{
    public class GetAllBookQuery : IRequest<Result<List<BookDto>>>, ICacheable
    {
        public string CacheKey => CacheKeys.Books.AllBooks;

        public TimeSpan CacheDuration => TimeSpan.FromMinutes(1);
    }
}
