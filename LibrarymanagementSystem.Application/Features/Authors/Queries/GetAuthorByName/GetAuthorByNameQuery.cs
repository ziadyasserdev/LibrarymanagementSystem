using LibrarymanagementSystem.Application.Common.Caching;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Authors.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorByName
{
    public class GetAuthorByNameQuery:IRequest<Result<AuthorDto>>,ICacheable
    {
        public string Name { get; set; }

        public string CacheKey => CacheKeys.Authors.ByName(Name);

        public TimeSpan CacheDuration => TimeSpan.FromMinutes(1);

        public GetAuthorByNameQuery(string Name)
        {
            this.Name = Name;
        }
    }
}
