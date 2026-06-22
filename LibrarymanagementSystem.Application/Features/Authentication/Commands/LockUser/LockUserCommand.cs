using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authentication.Commands.LockUser
{
    public class LockUserCommand : IRequest<Result<string>>
    {
        public string UserId { get; set; }

        public LockUserCommand(string userId)
        {
            UserId = userId;
        }
    }
}
