using LibrarymanagementSystem.Application.Common.Caching;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Authors.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.GetAllAuthors
{
    public class GetAllAuthorQuery : IRequest<Result<List<AuthorDto>>>, ICacheable
    {
        public string CacheKey => CacheKeys.Authors.All;

        public TimeSpan CacheDuration => TimeSpan.FromMinutes(1);
    }
}
