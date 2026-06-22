using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.ApplicationUsers.Commands.RestoreUserByAdmin
{
    public class RestoreUserByAdminCommand:IRequest<Result<string>>
    {
        public string UserId { get; set; }
        public RestoreUserByAdminCommand(string u)
        {
            UserId = u;
        }
    }
}
