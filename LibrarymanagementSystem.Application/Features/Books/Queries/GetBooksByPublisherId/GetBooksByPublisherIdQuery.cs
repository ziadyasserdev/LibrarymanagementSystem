using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksByPublisherId
{
    public class GetBooksByPublisherIdQuery:IRequest<Result<List<BookDto>>>
    {
        public int Id { get; set; }
        public GetBooksByPublisherIdQuery(int id)
        {
            this.Id = id;
        }
    }
}
