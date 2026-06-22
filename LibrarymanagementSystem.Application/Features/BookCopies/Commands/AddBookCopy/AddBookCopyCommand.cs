using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Data.Enum;
using LibrarymanagementSystem.Data.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Commands.AddBookCopy
{

    public class AddBookCopyCommand: IRequest<Result<int>>
    {

        public string Barcode { get; set; } = null!;
        public int BookId { get; set; }
        public int LocationId { get; set; }
    }
}
