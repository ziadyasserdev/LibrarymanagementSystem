using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authorizations.Commands.DeleteRole
{
    public class DeleteRoleCommand:IRequest<Result<string>>
    {
        public string RoleId { get; set; }
        public DeleteRoleCommand(string RoleId)
        {
            this.RoleId = RoleId;
        }
    }
}
