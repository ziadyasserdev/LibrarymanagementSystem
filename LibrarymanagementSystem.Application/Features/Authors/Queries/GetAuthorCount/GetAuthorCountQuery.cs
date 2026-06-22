using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorCount
{
    public class GetAuthorCountQuery:IRequest<Result<int>>
    {
        public bool? IsDeleted { get; set; }
        public GetAuthorCountQuery(bool? IsDeleted)
        {
            this.IsDeleted = IsDeleted; 
        }
    }
}
