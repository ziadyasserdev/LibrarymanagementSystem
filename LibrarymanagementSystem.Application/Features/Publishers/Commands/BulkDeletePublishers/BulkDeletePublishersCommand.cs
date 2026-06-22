using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Commands.BulkDeletePublishers
{
    public class BulkDeletePublishersCommand:IRequest<Result<string>>
    {
        public List<int> PublisherIds { get; set; }
        public BulkDeletePublishersCommand(List<int> PublisherIds)
        {
            this.PublisherIds = PublisherIds;
        }
    }
}
