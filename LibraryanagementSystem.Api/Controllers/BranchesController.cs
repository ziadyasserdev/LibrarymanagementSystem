using LibrarymanagementSystem.Api.Common.Responses;
using LibrarymanagementSystem.Application.Features.Books.Commands.UpdateBook;
using LibrarymanagementSystem.Application.Features.Branches.Commands.ActivateBranchesBulk;
using LibrarymanagementSystem.Application.Features.Branches.Commands.CreateBranch;
using LibrarymanagementSystem.Application.Features.Branches.Commands.DeactivateBranch;
using LibrarymanagementSystem.Application.Features.Branches.Commands.DeactivateBranchesBulk;
using LibrarymanagementSystem.Application.Features.Branches.Commands.DeleteBranch;
using LibrarymanagementSystem.Application.Features.Branches.Commands.DeleteBranchesBulk;
using LibrarymanagementSystem.Application.Features.Branches.Commands.EditBranch;
using LibrarymanagementSystem.Application.Features.Branches.Commands.RestoreBranch;
using LibrarymanagementSystem.Application.Features.Branches.Commands.RestoreBranchesBulk;
using LibrarymanagementSystem.Application.Features.Branches.Commands.SetBranchActive;
using LibrarymanagementSystem.Application.Features.Branches.Queries.GetAllBranchesForAdmin;
using LibrarymanagementSystem.Application.Features.Branches.Queries.GetAllBranchesForUsers;
using LibrarymanagementSystem.Application.Features.Branches.Queries.GetAllBranchesWithPaginationForAdmin;
using LibrarymanagementSystem.Application.Features.Branches.Queries.GetAllBranchesWithPaginationForUsers;
using LibrarymanagementSystem.Application.Features.Branches.Queries.GetBranchById;
using LibrarymanagementSystem.Application.Features.Branches.Queries.GetBranchDetails;
using LibrarymanagementSystem.Application.Features.Branches.Queries.GetBranchesStatistics;
using LibrarymanagementSystem.Application.Features.Branches.Queries.GetBranchStatistics;
using LibrarymanagementSystem.Application.Features.Branches.Queries.GetDeletedBranches;
using LibrarymanagementSystem.Application.Features.Branches.Queries.SearchBranches;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LibrarymanagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : ControllerBase
    {
        private readonly IMediator mediator;

        public BranchesController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet("ForAdmin")]
        //[Authorize(Roles = "Admin")]
        [SwaggerOperation(
      Summary = "Get all branches for admin",
      Description = "Retrieves all branches with full details, including inactive or soft-deleted branches, for admin purposes."
  )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBranchesForAdmin()
        {
            var result = await mediator.Send(new GetAllBranchesForAdminQuery());
            return result.ToActionResult();
        }

        [HttpGet("ForUsers")]
        [SwaggerOperation(
     Summary = "Get all active branches for users",
     Description = "Retrieves all active branches available for users. Only active branches are returned with limited details."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBranchesForUsers()
        {
            var result = await mediator.Send(new GetAllBranchesForUsersQuery());
            return result.ToActionResult();
        }
        [HttpGet("search")]
        [SwaggerOperation(
            Summary = "Search branches",
            Description = "Searches branches by name, city, and activation status with optional pagination."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchBranches(
            [FromQuery] SearchBranchesQuery query)
        {
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("{id}/details")]
        [SwaggerOperation(
            Summary = "Get branch details",
            Description = "Retrieves detailed information about a specific branch by its Id."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBranchDetail(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid branch Id.");

            var result = await mediator.Send(new GetBranchDetailsQuery(id));
            return result.ToActionResult();
        }


        [HttpGet("{id}")]
        [SwaggerOperation(
       Summary = "Get branch by Id",
       Description = "Retrieves the details of a specific branch using its Id."
   )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
          

            var result = await mediator.Send(new GetBranchByIdQuery(id));
            return result.ToActionResult();
        }


        [HttpGet("ForAdmin/Pagination")]
        [SwaggerOperation(
     Summary = "Get paginated branches for admin",
     Description = "Retrieves paginated branches with optional filtering by branch status."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBranchesForAdminPagination(
     [FromQuery] int pageNumber = 1,
     [FromQuery] int pageSize = 10,
     [FromQuery] BranchStatus branchStatus = BranchStatus.All)
        {
           

            var result = await mediator.Send(new GetAllBranchesWithPaginationForAdminQuery(pageNumber, pageSize, branchStatus));
            return result.ToActionResult();
        }

        [HttpGet("ForUsers/Pagination")]
        [SwaggerOperation(
    Summary = "Get paginated branches for users",
    Description = "Retrieves paginated active branches with filtering, searching, and sorting options."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBranchesForUsersPagination(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? search = null,
    [FromQuery] string? city = null,
    [FromQuery] BranchFilter branchFilter = BranchFilter.ByName,
    [FromQuery] BranchSort branchSort = BranchSort.Ascending)
        {
            

            var query = new GetAllBranchesWithPaginationForUsersQuery(pageNumber, pageSize)
            {
                Search = search,
                City = city,
                BranchFilter = branchFilter,
                BranchSort = branchSort
            };

            var result = await mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("statistics")]
        [SwaggerOperation(
      Summary = "Get branches statistics",
      Description = "Retrieves summary statistics for branches, such as total count, active count, and inactive count."
  )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStatistics()
        {
            var result = await mediator.Send(new GetBranchesStatisticsQuery());
            return result.ToActionResult();
        }

        [HttpPatch("{id}/activate")]
        [SwaggerOperation(
      Summary = "Activate a branch",
      Description = "Sets the specified branch as active."
  )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetActive(
      int id,
      [FromBody] SetBranchActiveCommand setBranchActiveCommand)
        {
           

            if (id != setBranchActiveCommand.Id)
                return BadRequest("Id mismatch.");

            var result = await mediator.Send(setBranchActiveCommand);
            return result.ToActionResult();
        }

        [HttpPatch("{id}/deactivate")]
        [SwaggerOperation(
     Summary = "Deactivate a branch",
     Description = "Sets the specified branch as inactive."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetInActive(
     int id,
     [FromBody] DeactivateBranchCommand deactivateBranchCommand)
        {


            if (id != deactivateBranchCommand.Id)
                return BadRequest("Id mismatch.");

            var result = await mediator.Send(deactivateBranchCommand);
            return result.ToActionResult();
        }


        [HttpPost]
        [SwaggerOperation(
      Summary = "Add a new branch",
      Description = "Creates a new branch in the system with the provided details."
  )]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddBranch([FromBody] CreateBranchCommand command)
        {
            if (command == null)
                return BadRequest("Command cannot be null.");

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPut("{id}/restore")]
        [SwaggerOperation(
            Summary = "Restore a deleted branch",
            Description = "Restores a previously soft-deleted branch using its Id."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RestoreBranch(
            int id,
            [FromBody] RestoreBranchCommand command)
        {
            if (command == null)
                return BadRequest("Command cannot be null.");

            if (id != command.Id)
                return BadRequest("Id mismatch.");

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpGet("deleted")]
        //[Authorize(Roles = "Admin")]
        [SwaggerOperation(
      Summary = "Get deleted branches",
      Description = "Retrieves paginated list of soft-deleted branches for admin purposes."
  )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDeletedBranch(
      [FromQuery] int pageNumber = 1,
      [FromQuery] int pageSize = 10)
        {
           
          

            var result = await mediator.Send(new GetDeletedBranchesQuery(pageNumber, pageSize));
            return result.ToActionResult();
        }

        [HttpGet("{id}/statistics")]
        [SwaggerOperation(
      Summary = "Get branch statistics",
      Description = "Retrieves detailed statistics for a specific branch using its Id."
  )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBranchStatistics(int id)
        {
           

            var result = await mediator.Send(new GetBranchStatisticsQuery(id));
            return result.ToActionResult();
        }

        [HttpDelete("bulk")]
    //  [Authorize(Roles = "Admin")]
        [SwaggerOperation(
     Summary = "Bulk delete branches",
     Description = "Deletes multiple branches using a list of branch IDs."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteBulk(
     [FromBody] DeleteBranchesBulkCommand command)
        {

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPatch("bulk/activate")]
       // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
     Summary = "Bulk activate branches",
     Description = "Activates multiple branches using a list of branch IDs."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkActivate(
     [FromBody] ActivateBranchesBulkCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPatch("bulk/deactivate")]
        //[Authorize(Roles = "Admin")]
        [SwaggerOperation(
     Summary = "Bulk deactivate branches",
     Description = "Deactivates multiple branches using a list of branch IDs."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkDeactivate(
     [FromBody] DeactivateBranchesBulkCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPatch("bulk/restore")]
       // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Bulk restore branches",
    Description = "Restores multiple soft-deleted branches using a list of branch IDs."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkRestore(
    [FromBody] RestoreBranchesBulkCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }







        [HttpPut]
        [SwaggerOperation(
       Summary = "Update a branch",
       Description = "Updates the details of an existing branch."
   )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBranch([FromBody] EditBranchCommand command)
        {
            

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpDelete]
        [SwaggerOperation(
      Summary = "Delete a branch",
      Description = "Deletes an existing branch from the system using the branch's ID."
  )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBranch([FromBody] DeleteBranchCommand command)
        {
            

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

    }
}
