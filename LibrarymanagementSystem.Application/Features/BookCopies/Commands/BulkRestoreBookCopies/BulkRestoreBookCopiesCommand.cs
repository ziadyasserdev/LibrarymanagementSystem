using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Commands.BulkRestoreBookCopies
{
    public class BulkRestoreBookCopiesCommand : IRequest<Result<string>>
    {
        public List<int> Ids { get; set; } = new();
        public BulkRestoreBookCopiesCommand(List<int> Ids)
        {
            this.Ids = Ids; 
        }
    }
}
