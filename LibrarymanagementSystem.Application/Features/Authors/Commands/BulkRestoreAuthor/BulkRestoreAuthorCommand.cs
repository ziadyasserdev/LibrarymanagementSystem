using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Commands.BulkRestoreAuthor
{
    public class BulkRestoreAuthorCommand : IRequest<Result<string>>
    {
        public List<int> AuthorIds { get; set; }
        public BulkRestoreAuthorCommand(List<int> AuthorIds)
        {
            this.AuthorIds = AuthorIds;
        }
    }
}
