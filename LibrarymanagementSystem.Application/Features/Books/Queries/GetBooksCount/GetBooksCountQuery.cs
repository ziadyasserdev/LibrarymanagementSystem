using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksCount
{
    public class GetBooksCountQuery : IRequest<Result<int>>
    {
        
        public bool? IncludeDeleted { get; set; } = false;
    }
}
