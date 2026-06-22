using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Branches.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Queries.GetAllBranchesWithPaginationForAdmin
{
    public class GetAllBranchesWithPaginationForAdminQuery:IRequest<Result<PaginatedResult<AdminBranchDto>>>
    {
            public int PageSize { get; set; }
            public int PageNumber { get; set; }
        public BranchStatus BranchStatus { get; set; }
        public GetAllBranchesWithPaginationForAdminQuery(int pn,int ps,BranchStatus branchStatus)
        {
            this.PageNumber = pn;
            this.PageSize = ps;
            this.BranchStatus = branchStatus;
        }
    }
}
