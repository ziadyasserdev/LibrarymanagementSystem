using LibrarymanagementSystem.Application.Common.Caching;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Authors.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorByCategoryName
{
    public class GetAuthorByCategoryNameQuery:IRequest<Result<List<AuthorDto>>>,ICacheable
    {
        public string CategoryName { get; set; }

        public string CacheKey => CacheKeys.Authors.ByCategory(CategoryName);

        public TimeSpan CacheDuration => TimeSpan.FromMinutes(1);

        public GetAuthorByCategoryNameQuery(string CategoryName)
        {
            this.CategoryName = CategoryName;
        }

    }
}
