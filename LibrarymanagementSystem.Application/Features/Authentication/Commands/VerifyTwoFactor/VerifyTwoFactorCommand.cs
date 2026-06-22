using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Authentication.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authentication.Commands.VerifyTwoFactor
{
    public class VerifyTwoFactorCommand:IRequest<Result<AuthTokenDto>>
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
