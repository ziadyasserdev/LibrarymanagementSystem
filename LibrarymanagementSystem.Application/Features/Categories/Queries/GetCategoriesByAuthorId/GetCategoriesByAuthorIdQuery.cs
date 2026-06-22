using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Categories.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Queries.GetCategoriesByAuthorId
{
    public class GetCategoriesByAuthorIdQuery:IRequest<Result<List<CategoryDto>>>
    {
        public int AuthorId { get; set; }
        public GetCategoriesByAuthorIdQuery(int AuthorId)
        {
            this.AuthorId = AuthorId;
        }
    }
}
