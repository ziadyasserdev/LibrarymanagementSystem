using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.ApplicationUsers.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.ApplicationUsers.Queries.GetAllUsersWithPagination
{
    public class GetAllUsersWithPaginationQuery:IRequest<Result<PaginatedResult<ApplicationUserDto>>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public GetAllUsersWithPaginationQuery(int PageSize, int PageNumber)
        {
            this.PageSize = PageSize;
            this.PageNumber= PageNumber;
        }
    }
}
