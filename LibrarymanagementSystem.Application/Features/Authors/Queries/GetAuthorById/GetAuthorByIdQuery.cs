using LibrarymanagementSystem.Application.Common.Caching;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Authors.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorById
{
    public class GetAuthorByIdQuery:IRequest<Result<AuthorDto>>,ICacheable
    {
        public int Id { get; set; }

        public string CacheKey => CacheKeys.Authors.ById(Id);

        public TimeSpan CacheDuration => TimeSpan.FromMinutes(1);

        public GetAuthorByIdQuery(int Id)
        {
            this.Id = Id;
        }
    }
}
