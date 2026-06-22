using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.DeleteBookFile
{
    public class DeleteBookFileCommand:IRequest<Result<int>>
    {
        public int BookId { get; set; }
        public DeleteBookFileCommand(int BookId)
        {
            this.BookId = BookId;
        }
    }
}
