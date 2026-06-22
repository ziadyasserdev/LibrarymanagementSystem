using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.ApplicationUsers.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.ApplicationUsers.Queries.GetAllUsers
{
    public class GetAllUsersQuery:IRequest<Result<List<ApplicationUserDto>>>
    {
    }
}
