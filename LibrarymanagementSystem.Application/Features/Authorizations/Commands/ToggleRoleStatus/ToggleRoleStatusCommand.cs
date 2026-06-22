using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authorizations.Commands.ToggleRoleStatus
{
    public class ToggleRoleStatusCommand:IRequest<Result<string>>
    {
        public string roleId { get; set; }  
      
    }
}
