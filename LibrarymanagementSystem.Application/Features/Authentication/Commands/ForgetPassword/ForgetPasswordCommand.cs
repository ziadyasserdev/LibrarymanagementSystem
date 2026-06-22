using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authentication.Commands.ForgetPassword
{
    public class ForgetPasswordCommand:IRequest<Result<string>>
    {
         public string Email { get; set; }
    }
}
