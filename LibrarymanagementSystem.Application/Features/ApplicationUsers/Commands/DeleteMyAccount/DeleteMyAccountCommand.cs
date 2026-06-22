using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.ApplicationUsers.Commands.DeleteMyAccount
{
    public class DeleteMyAccountCommand:IRequest<Result<string>>
    {
    }
}
