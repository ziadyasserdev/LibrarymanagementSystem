using LibrarymanagementSystem.Application.Common.Caching;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Authors.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Commands.CreateAuthor
{
    public class CreateAuthorCommand:IRequest<Result<AuthorDto>>,IInvalidateCache
    {
        public string Name { get; set; }
        public string Biography { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string[] CacheKeysToInvalidate => new[] { CacheKeys.Authors.All };
    }
}
