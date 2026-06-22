using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksByCategoryId
{
    public class GetBooksByCategoryIdQuery:IRequest<Result<List<BookDto>>>
    {
        public int CategoryId { get; set; }
        public GetBooksByCategoryIdQuery(int id)
        {
            this.CategoryId = id;
        }
    }
}
