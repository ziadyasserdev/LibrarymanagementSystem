using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Commands.BulkDeleteAuthor
{
    public class BulkDeleteAuthorCommand : IRequest<Result<string>>
    {
        public List<int> AuthorIds { get; set; }
        public BulkDeleteAuthorCommand(List<int> AuthorIds)
        {
            this.AuthorIds = AuthorIds;
        }

    }
}
   
