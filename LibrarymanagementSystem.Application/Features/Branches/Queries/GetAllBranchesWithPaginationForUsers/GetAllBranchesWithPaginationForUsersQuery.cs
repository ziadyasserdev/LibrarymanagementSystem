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

namespace LibrarymanagementSystem.Application.Features.Branches.Queries.GetAllBranchesWithPaginationForUsers
{
    public class GetAllBranchesWithPaginationForUsersQuery:IRequest<Result<PaginatedResult<BranchUserDto>>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        public string? Search { get; set; }
        public string? City { get; set; }

        public BranchFilter BranchFilter { get; set; } = BranchFilter.ByName;
        public BranchSort BranchSort { get; set; } = BranchSort.Ascending;

        public GetAllBranchesWithPaginationForUsersQuery(int pn, int ps)
        {
            PageNumber = pn;
            PageSize = ps;
        }
    }
}
