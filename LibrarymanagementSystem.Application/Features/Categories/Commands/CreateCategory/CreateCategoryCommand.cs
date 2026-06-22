using LibrarymanagementSystem.Application.Common.Caching;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Categories.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand:IRequest<Result<CategoryDto>>,IInvalidateCache
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string[] CacheKeysToInvalidate => new[] { CacheKeys.Categories.All};
    }
}
