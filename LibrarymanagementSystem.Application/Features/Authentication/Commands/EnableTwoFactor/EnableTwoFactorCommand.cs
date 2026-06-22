using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authentication.Commands.EnableTwoFactor
{
    public class EnableTwoFactorCommand:IRequest<Result<string>>
    {
        public string UserId { get; set; }
    }
}
