using LibrarymanagementSystem.Application.Common.Caching;
using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommand:IRequest<Result<int>>,IInvalidateCache
    {
        public int Id { get; set; }
        public DeleteCategoryCommand(int Id)
        {
            this.Id = Id;
        }
        public string[] CacheKeysToInvalidate => new[] {
            CacheKeys.Categories.All,
            CacheKeys.Categories.ById(Id)};

    }
}
