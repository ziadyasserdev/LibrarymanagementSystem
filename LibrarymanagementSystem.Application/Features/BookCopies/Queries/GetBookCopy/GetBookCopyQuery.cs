using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Queries.GetBookCopy
{
    public class GetBookCopyQuery:IRequest<Result<object>>
    {
        public int Id { get; set; }
        public GetBookCopyQuery(int id)
        {
            Id = id;
        }
    }
}
