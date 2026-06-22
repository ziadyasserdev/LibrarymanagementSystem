using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Commands.EditBookCopy { 
    public class EditBookCopyCommand:IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Barcode { get; set; } = null!;
        public int BookId { get; set; }
        public int LocationId { get; set; }
    }
}
