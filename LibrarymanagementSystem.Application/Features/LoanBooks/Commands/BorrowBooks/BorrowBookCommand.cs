using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Commands.BorrowBooks
{
    public class BorrowBookCommand:IRequest<Result<int>>
    {
      
        public int BookCopy { get; set; }
         
    }
}
