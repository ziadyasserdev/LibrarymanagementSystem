using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.CanDeleteBook
{
    public class CanDeleteBookQuery : IRequest<Result<bool>>
    {
        public int BookId { get; set; }

        public CanDeleteBookQuery(int bookId)
        {
            BookId = bookId;
        }
    }
}
