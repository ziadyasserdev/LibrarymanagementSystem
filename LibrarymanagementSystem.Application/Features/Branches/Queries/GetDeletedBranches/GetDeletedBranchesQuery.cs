using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Branches.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Queries.GetDeletedBranches
{
    public class GetDeletedBranchesQuery:IRequest<Result<PaginatedResult<DeletedBranchDto>>>
    {
           public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public GetDeletedBranchesQuery(int pn, int ps)
        {
            PageNumber=pn;
            PageSize=ps;
        }
    }
}
