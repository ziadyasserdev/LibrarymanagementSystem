using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Authorizations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authorizations.Commands.EditRole
{
    public class EditRoleCommand:IRequest<Result<RoleDto>>
    {
        public string roleId { get; set; }
        public string newRoleName {  get; set; }
      
    }
}
