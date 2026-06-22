using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Authors.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorsByCategoryId
{
    public class GetAuthorsByCategoryIdQuery:IRequest<Result<List<AuthorDto>>>
    {
        public int CategoryId { get; set; }
        public GetAuthorsByCategoryIdQuery(int CategoryId)
        {
            this.CategoryId = CategoryId;
        }
    }
}
