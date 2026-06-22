using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Commands.BulkDeactiveAuthor
{
    public class BulkDeactiveAuthorCommand:IRequest<Result<string>>
    {
        public List<int> AuthorIds { get; set; }
        public BulkDeactiveAuthorCommand(List<int> AuthorIds)
        {
            this.AuthorIds = AuthorIds;
        }
    }
}
