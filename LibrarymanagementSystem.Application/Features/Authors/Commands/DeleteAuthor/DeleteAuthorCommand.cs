using LibrarymanagementSystem.Application.Common.Caching;
using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Commands.DeleteAuthor
{
    public class DeleteAuthorCommand:IRequest<Result<int>> ,IInvalidateCache
    {
        public int Id { get; set; }

      

        public DeleteAuthorCommand(int Id)
        {
            this.Id = Id;
        }
        public string[] CacheKeysToInvalidate => new[] { CacheKeys.Authors.All,
                                                         CacheKeys.Authors.ById(Id),
                                                         };
    }
}
