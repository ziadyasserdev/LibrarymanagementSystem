using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksByAuthorId
{
    public class GetBooksByAuthorIdQuery:IRequest<Result<List<BookDto>>>
    {
        public int AuthorId { get; set; }
        public GetBooksByAuthorIdQuery(int a)
        {
            this.AuthorId = a;
        }
    }
}
