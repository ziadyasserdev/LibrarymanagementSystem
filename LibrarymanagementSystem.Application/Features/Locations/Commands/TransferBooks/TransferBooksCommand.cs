using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Commands.TransferBooks
{
    public class TransferBooksCommand:IRequest<Result<string>>
    {
       public List<int> BookCopiesId { get; set; }= new List<int>();

        public int FromLocationId { get; set; }

        public int ToLocationId { get; set; }

       
    }
}
