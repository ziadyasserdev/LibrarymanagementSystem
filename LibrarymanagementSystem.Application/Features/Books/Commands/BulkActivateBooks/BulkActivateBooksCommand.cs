using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.BulkActivateBooks
{
    public class BulkActivateBooksCommand:IRequest<Result<string>>
    {
        public List<int> BookIds { get; set; } = new();

        public BulkActivateBooksCommand(List<int> bookIds)
        {
            BookIds = bookIds;
        }
    }
}
