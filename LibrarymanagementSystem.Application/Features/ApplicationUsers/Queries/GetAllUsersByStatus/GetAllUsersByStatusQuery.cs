using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.ApplicationUsers.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.ApplicationUsers.Queries.GetAllUsersByStatus
{
    public class GetAllUsersByStatusQuery:IRequest<Result<PaginatedResult<ApplicationUserDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public UserStatus UserStatus { get; set; }
        public GetAllUsersByStatusQuery(int pn , int ps , UserStatus userStatus)
        {
            PageNumber=pn;
            PageSize=ps;
            UserStatus=userStatus;
        }
    }
}
