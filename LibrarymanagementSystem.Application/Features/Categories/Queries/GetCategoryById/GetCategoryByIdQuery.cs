using LibrarymanagementSystem.Application.Common.Caching;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Categories.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQuery:IRequest<Result<CategoryDto>>,ICacheable
    {
        public int Id { get; set; }

        public string CacheKey => CacheKeys.Categories.ById(Id);

        public TimeSpan CacheDuration => TimeSpan.FromMinutes(1);

        public GetCategoryByIdQuery(int Id)
        {
            this.Id = Id;
        }
    }
}
