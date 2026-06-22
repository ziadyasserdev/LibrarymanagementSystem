using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.BulkDeleteBooks
{
    public class BulkDeleteBooksCommand:IRequest<Result<string>>
    {
        public List<int> BookIds { get; set; } = new();

        public BulkDeleteBooksCommand(List<int> bookIds)
        {
            BookIds = bookIds;
        }
    }
}
