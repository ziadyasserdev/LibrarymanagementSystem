using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.BulkDeactivateBooks
{
    public class BulkDeactivateBooksCommand:IRequest<Result<string>>
    {
        public List<int> BookIds { get; set; } = new();

        public BulkDeactivateBooksCommand(List<int> bookIds)
        {
            BookIds = bookIds;
        }
    }
}
