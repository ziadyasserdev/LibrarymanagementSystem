using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Categories.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Queries.GetCategoriesByAuthorName
{
    public class GetCategoriesByAuthorNameQuery:IRequest<Result<List<CategoryDto>>>
    {
        public string AuthorName { get; set; }
        public GetCategoriesByAuthorNameQuery(string AuthorName)
        {
            this.AuthorName = AuthorName;
        }
    }
}
