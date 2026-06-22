using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authentication.Commands.RevokeToken
{
    public class RevokeTokenCommand:IRequest<Result<int>>
    {
        public string refreshToken { get; set; }
        public RevokeTokenCommand(string refreshToken)
        {
            this.refreshToken = refreshToken;
        }
    }
}
