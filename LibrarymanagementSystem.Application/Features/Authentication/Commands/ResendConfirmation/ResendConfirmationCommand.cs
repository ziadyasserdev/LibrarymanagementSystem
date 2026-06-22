using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authentication.Commands.ResendConfirmation
{
    public class ResendConfirmationCommand:IRequest<Result<string>>
    {
        public string Email { get; set; }
        public ResendConfirmationCommand(string email)
        {
            Email=email;
        }
    }
}
