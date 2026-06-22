using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Authentication.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authentication.Commands.Login
{
    public class LoginUserCommand:IRequest<Result<AuthTokenDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public LoginUserCommand(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }
    }
}
