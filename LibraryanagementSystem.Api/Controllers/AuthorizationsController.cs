using LibrarymanagementSystem.Api.Common.Responses;
using LibrarymanagementSystem.Application.Features.Authorizations.Commands.AddClaimToUser;
using LibrarymanagementSystem.Application.Features.Authorizations.Commands.AddRoleToUser;
using LibrarymanagementSystem.Application.Features.Authorizations.Commands.CreateRole;
using LibrarymanagementSystem.Application.Features.Authorizations.Commands.DeleteRole;
using LibrarymanagementSystem.Application.Features.Authorizations.Commands.EditRole;
using LibrarymanagementSystem.Application.Features.Authorizations.Commands.EditUserClaimValue;
using LibrarymanagementSystem.Application.Features.Authorizations.Commands.RemoveClaimFromUser;
using LibrarymanagementSystem.Application.Features.Authorizations.Commands.RemoveRoleFromUser;
using LibrarymanagementSystem.Application.Features.Authorizations.Commands.ToggleRoleStatus;
using LibrarymanagementSystem.Application.Features.Authorizations.Commands.UpdateRoleOfUser;
using LibrarymanagementSystem.Application.Features.Authorizations.Queries.GetAllRoles;
using LibrarymanagementSystem.Application.Features.Authorizations.Queries.GetAllRolesOfUser;
using LibrarymanagementSystem.Application.Features.Authorizations.Queries.GetAllRolesWithPagination;
using LibrarymanagementSystem.Application.Features.Authorizations.Queries.GetRoleById;
using LibrarymanagementSystem.Application.Features.Authorizations.Queries.IsRoleExist;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LibrarymanagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Manage roles and user authorization: roles, claims, assignments, updates and activation.")]
    public class AuthorizationsController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthorizationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // ===================== GET =====================

        [HttpGet("AllRoles")]
        [SwaggerOperation(
            Summary = "Get all roles",
            Description = "Retrieve a list of all roles in the system."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await mediator.Send(new GetAllRolesQuery());
            return result.ToActionResult();
        }

        [HttpGet("roles")]
        [SwaggerOperation(
     Summary = "Get roles with pagination",
     Description = "Retrieve roles using pagination parameters."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRoles(
     [FromQuery] int pageNumber = 1,
     [FromQuery] int pageSize = 10)
        {
            var result = await mediator.Send(new GetAllRolesWithPaginationQuery(pageNumber, pageSize));
            return result.ToActionResult();
        }
        [HttpGet("users/{userId}/roles")]
        [SwaggerOperation(
     Summary = "Get user roles",
     Description = "Retrieve all roles assigned to a specific user."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var result = await mediator.Send(new GetAllRolesOfUserQuery(userId));
            return result.ToActionResult();
        }

        [HttpGet("roles/{id}")]
        [SwaggerOperation(
      Summary = "Get role by ID",
      Description = "Retrieve role details using role ID."
  )]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var result = await mediator.Send(new GetRoleByIdQuery(id));
            return result.ToActionResult();
        }

        [HttpGet("roles/{id}/exists")]
        [SwaggerOperation(
      Summary = "Check if role exists",
      Description = "Check whether a role exists using its ID."
  )]
        public async Task<IActionResult> RoleExists(string id)
        {
            var result = await mediator.Send(new IsRoleExistQuery(id));
            return result.ToActionResult();
        }

        // ===================== POST =====================

        [HttpPost("AddRole")]
        [SwaggerOperation(
            Summary = "Create new role",
            Description = "Create a new role in the system."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddRole([FromQuery] string roleName)
        {
            var result = await mediator.Send(new CreateRoleCommand(roleName));
            return result.ToActionResult();
        }

        [HttpPost("AddRoleToUser")]
        [SwaggerOperation(
            Summary = "Assign role to user",
            Description = "Assign an existing role to a specific user."
        )]
        public async Task<IActionResult> AssignRoleToUser(AddRoleToUserCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPost("AddClaimToUser")]
        [SwaggerOperation(
            Summary = "Assign claim to user",
            Description = "Assign a claim to a specific user."
        )]
        public async Task<IActionResult> AssignClaimToUser(AddClaimToUserCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        // ===================== PUT =====================

        [HttpPut("Role/{id}")]
        [SwaggerOperation(
            Summary = "Update role",
            Description = "Update role information using role ID."
        )]
        public async Task<IActionResult> UpdateRole(string id, EditRoleCommand command)
        {
            if (id != command.roleId)
                return BadRequest("Id mismatched");

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPut("UserRole/{id}")]
        [SwaggerOperation(
            Summary = "Update user role",
            Description = "Update role assigned to a specific user."
        )]
        public async Task<IActionResult> UpdateUserRole(string id, UpdateRoleOfUserCommand command)
        {
            if (id != command.UserId)
                return BadRequest("Id mismatched");

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPut("UserClaim/{id}")]
        [SwaggerOperation(
            Summary = "Update user claim",
            Description = "Update claim value assigned to a specific user."
        )]
        public async Task<IActionResult> UpdateUserClaim(string id, EditUserClaimValueCommand command)
        {
            if (id != command.UserId)
                return BadRequest("Id mismatched");

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        // ===================== PATCH =====================

        [HttpPatch("MakeRoleActive/{id}")]
        [SwaggerOperation(
            Summary = "Toggle role status",
            Description = "Activate or deactivate a role."
        )]
        public async Task<IActionResult> MakeRoleActive(string id, ToggleRoleStatusCommand command)
        {
            if (id != command.roleId)
                return BadRequest("Id mismatched");

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        // ===================== DELETE =====================

        [HttpDelete("Role/{id}")]
        [SwaggerOperation(
            Summary = "Delete role",
            Description = "Delete a role using role ID."
        )]
        public async Task<IActionResult> DeleteRole(string id, DeleteRoleCommand command)
        {
            if (id != command.RoleId)
                return BadRequest("Id mismatched");

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpDelete("RoleFromUser/{id}")]
        [SwaggerOperation(
            Summary = "Remove role from user",
            Description = "Remove a specific role from a user."
        )]
        public async Task<IActionResult> RemoveRoleFromUser(string id, RemoveRoleFromUserCommand command)
        {
            if (id != command.userId)
                return BadRequest("Id mismatched");

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpDelete("ClaimFromUser/{id}")]
        [SwaggerOperation(
            Summary = "Remove claim from user",
            Description = "Remove a specific claim from a user."
        )]
        public async Task<IActionResult> RemoveClaimFromUser(string id, RemoveClaimFromUserCommand command)
        {
            if (id != command.userId)
                return BadRequest("Id mismatched");

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
    }

}
