using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Authorizations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authorizations.Queries.GetAllRolesOfUser
{
    public class GetAllRolesOfUserQuery:IRequest<Result<List<UserRoleDto>>>
    {
        public string UserId { get; set; }
        public GetAllRolesOfUserQuery(string u)
        {
            UserId = u;
        }
    }
}
