using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.BookCopies.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Queries.CanDeleteBookCopy
{
    public class CanDeleteBookCopyQuery : IRequest<Result<CanDeleteBookCopyDto>>
    {
        public int Id { get; set; }

        public CanDeleteBookCopyQuery(int id)
        {
            Id = id;
        }
    }
}
