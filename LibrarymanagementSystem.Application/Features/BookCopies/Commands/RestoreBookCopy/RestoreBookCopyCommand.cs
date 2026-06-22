using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Commands.RestoreBookCopy
{
    public class RestoreBookCopyCommand:IRequest<Result<int>>
    {
        public int Id { get; set; }
        public RestoreBookCopyCommand(int id)
        {
            Id = id;
        }
    }
}
