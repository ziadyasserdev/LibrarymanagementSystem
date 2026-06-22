using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authentication.Commands.UnlockUser
{
    public class UnlockUserCommand : IRequest<Result<string>>
    {
        public string UserId { get; set; }
        public UnlockUserCommand(string userId)
        {
            UserId = userId;
        }
    }
}
