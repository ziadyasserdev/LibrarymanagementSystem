using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Authorizations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authorizations.Queries.GetAllRolesWithPagination
{
    public class GetAllRolesWithPaginationQuery:IRequest<Result<PaginatedResult<RoleDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public GetAllRolesWithPaginationQuery(int pn,int ps)
        {
            PageNumber = pn;
            PageSize = ps;
        }
    }
}
