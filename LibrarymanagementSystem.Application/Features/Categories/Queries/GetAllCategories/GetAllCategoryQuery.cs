using LibrarymanagementSystem.Application.Common.Caching;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Categories.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Queries.GetAllCategories
{
    public class GetAllCategoryQuery : IRequest<Result<List<CategoryDto>>>, ICacheable
    {
        public string CacheKey => CacheKeys.Categories.All;

        public TimeSpan CacheDuration => TimeSpan.FromMinutes(1);
    }
}
