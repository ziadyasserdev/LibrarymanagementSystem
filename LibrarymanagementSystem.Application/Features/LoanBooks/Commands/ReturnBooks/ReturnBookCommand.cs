using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Commands.ReturnBooks
{
    public class ReturnBookCommand:IRequest<Result<int>>
    {
        public int BookCopyId { get; set; }
        public ReturnBookCommand(int Id)
        {
            this.BookCopyId = Id;
        }
    }
}
