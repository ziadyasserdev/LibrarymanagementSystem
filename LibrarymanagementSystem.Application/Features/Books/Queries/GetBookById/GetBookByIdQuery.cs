using LibrarymanagementSystem.Application.Common.Caching;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetBookById
{
    public class GetBookByIdQuery:IRequest<Result<BookDto>>,ICacheable
    {
        public int Id { get; set; }

        public string CacheKey => CacheKeys.Books.BookById(Id);

        public TimeSpan CacheDuration => TimeSpan.FromMinutes(1);

        public GetBookByIdQuery(int Id)
        {
            this.Id = Id;
        }
    }
}
