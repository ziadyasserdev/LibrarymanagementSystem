using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Authentication.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authentication.Commands.RefreshToken
{
    public class RefreshTokenCommand:IRequest<Result<AuthTokenDto>>
    {
         public string refreshToken { get; set; }
    }
}
