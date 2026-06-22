using LibrarymanagementSystem.Api.Common.Responses;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;

using LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetAllLoanBookWithPagination;
using LibrarymanagementSystem.Application.Features.Locations.Commands.BulkActivateLocations;
using LibrarymanagementSystem.Application.Features.Locations.Commands.BulkDeactivateLocations;
using LibrarymanagementSystem.Application.Features.Locations.Commands.BulkRestoreLocations;
using LibrarymanagementSystem.Application.Features.Locations.Commands.BulkSoftDeleteLocations;
using LibrarymanagementSystem.Application.Features.Locations.Commands.CreateLocation;
using LibrarymanagementSystem.Application.Features.Locations.Commands.DeleteLocation;
using LibrarymanagementSystem.Application.Features.Locations.Commands.MakeLocationActive;
using LibrarymanagementSystem.Application.Features.Locations.Commands.MakeLocationInActive;
using LibrarymanagementSystem.Application.Features.Locations.Commands.RestoreLocation;
using LibrarymanagementSystem.Application.Features.Locations.Commands.TransferBooks;
using LibrarymanagementSystem.Application.Features.Locations.Commands.UpdateLocation;
using LibrarymanagementSystem.Application.Features.Locations.Dtos;
using LibrarymanagementSystem.Application.Features.Locations.Queries.CanDeleteLocation;
using LibrarymanagementSystem.Application.Features.Locations.Queries.GetAllActiveLocations;
using LibrarymanagementSystem.Application.Features.Locations.Queries.GetAllInActiveLocations;
using LibrarymanagementSystem.Application.Features.Locations.Queries.GetAllLocations;
using LibrarymanagementSystem.Application.Features.Locations.Queries.GetAllLocationsWithPagination;
using LibrarymanagementSystem.Application.Features.Locations.Queries.GetEmptyLocations;
using LibrarymanagementSystem.Application.Features.Locations.Queries.GetLocationById;
using LibrarymanagementSystem.Application.Features.Locations.Queries.GetLocationCapacity;
using LibrarymanagementSystem.Application.Features.Locations.Queries.GetLocationDetails;
using LibrarymanagementSystem.Application.Features.Locations.Queries.GetLocationsByBranch;
using LibrarymanagementSystem.Application.Features.Locations.Queries.GetLocationsCount;
using LibrarymanagementSystem.Application.Features.Locations.Queries.GetLocationStatistics;
using LibrarymanagementSystem.Application.Features.Locations.Queries.GetSpecificLocationStatistics;
using LibrarymanagementSystem.Application.Features.Locations.Queries.SearchLocations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Swashbuckle.AspNetCore.Annotations;

namespace LibrarymanagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly IMediator mediator;

        public LocationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [SwaggerOperation(
    Summary = "Get all locations",
    Description = "Retrieve all locations from the system."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
   
        {
            var result = await mediator.Send(new GetAllLocationsQuery());

            return result.ToActionResult();
        }

        [HttpGet("all/active")]
        [SwaggerOperation(
            Summary = "Get all active locations",
            Description = "Retrieve all locations that are currently active."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllActive()
        {
            var result = await mediator.Send(new GetAllActiveLocationsQuery());
            return result.ToActionResult();
        }

        [HttpGet("all/inactive")]
        [SwaggerOperation(
            Summary = "Get all inactive locations",
            Description = "Retrieve all locations that are currently inactive."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllInActive()
        {
            var result = await mediator.Send(new GetAllInActiveLocationsQuery());
            return result.ToActionResult();
        }

      

        [HttpGet("pagination")]
        [SwaggerOperation(
     Summary = "Get all locations with pagination",
     Description = "Retrieve locations using pagination by specifying page number and page size."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllWithPagination(int pageNumber, int pageSize)
        {
            var result = await mediator.Send(
                new GetAllLocationsWithPaginationQuery() { PageNumber=pageNumber,PageSize=pageSize});

            return result.ToActionResult();
        }


     

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get location by Id",
            Description = "Retrieve a specific location by its Id."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await mediator.Send(new GetLocationByIdQuery(id));

            return result.ToActionResult();
        }



        [HttpPost]
        [SwaggerOperation(
     Summary = "Add a new location",
     Description = "Create a new location (Shelf, Floor, Section) in the library."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateLocation([FromBody] CreateLocationCommand createLocationCommand)
        {
            

            var result = await mediator.Send(createLocationCommand);

            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update an existing location",
            Description = "Update a location's Shelf, Floor, or Section by its Id."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateLocation(int id, [FromBody] UpdateLocationCommand updateLocationCommand)
        {
           
            if (id != updateLocationCommand.Id)
                return BadRequest("Id mismatch.");

           
            var result = await mediator.Send(updateLocationCommand);

       
            return result.ToActionResult();
        }


        [HttpPut("make/active/{id}")]
        [SwaggerOperation(
    Summary = "Activate a location",
    Description = "Set a location as active by its Id."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MakeLocationActive(
    int id,
    [FromBody] MakeLocationActiveCommand makeLocationActiveCommand)
        {
            if (id != makeLocationActiveCommand.LocationId)
                return BadRequest("Id mismatch.");

            var result = await mediator.Send(makeLocationActiveCommand);

            return result.ToActionResult();
        }


        [HttpPut("make/inactive/{id}")]
        [SwaggerOperation(
     Summary = "Deactivate a location",
     Description = "Set a location as inactive by its Id."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MakeLocationInActive(
     int id,
     [FromBody] MakeLocationInActiveCommand makeLocationInActiveCommand)
        {
            if (id != makeLocationInActiveCommand.LocationId)
                return BadRequest("Id mismatch.");

            var result = await mediator.Send(makeLocationInActiveCommand);

            return result.ToActionResult();
        }

        [HttpPatch("restore")]
        //[Authorize(Roles = "Admin")]
        [SwaggerOperation(
     Summary = "Restore a soft-deleted location",
     Description = "Restores a previously soft-deleted location using its Id."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RestoreLocation(
     [FromBody] RestoreLocationCommand restoreLocationCommand)
        {
            if (restoreLocationCommand == null)
                return BadRequest("Command cannot be null.");

            var result = await mediator.Send(restoreLocationCommand);
            return result.ToActionResult();
        }


        [HttpGet("count")]
        [SwaggerOperation(
     Summary = "Get total locations count",
     Description = "Retrieves the total number of locations in the system."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLocationCount()
        {
            var result = await mediator.Send(new GetLocationsCountQuery());
            return result.ToActionResult();
        }

        [HttpGet("{branchId:int}/locations")]
        [SwaggerOperation(
     Summary = "Get locations by branch",
     Description = "Retrieves all locations that belong to a specific branch."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLocationsByBranchId(int branchId)
        {
            

            var result = await mediator.Send(
                new GetLocationsByBranchQuery(branchId));

            return result.ToActionResult();
        }

        [HttpGet("{id}/details")]
        [SwaggerOperation(
     Summary = "Get location details",
     Description = "Retrieves detailed information about a specific location by its Id."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLocationsDetails(int id)
        {
          

            var result = await mediator.Send(new GetLocationDetailsQuery(id));
            return result.ToActionResult();
        }

        [HttpGet("empty")]
        [SwaggerOperation(
    Summary = "Get empty locations",
    Description = "Retrieves all locations that currently have no books assigned or are empty."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmptyLocations()
        {
            var result = await mediator.Send(new GetEmptyLocationsQuery());
            return result.ToActionResult();
        }

        [HttpPost("transfer-books")]
       // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
       Summary = "Transfer books between locations",
       Description = "Transfers a specified number of books from one location to another."
   )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> TransferBooks(
       [FromBody] TransferBooksCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPatch("bulk/activate")]
        //[Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Bulk activate locations",
    Description = "Activates multiple locations using a list of location IDs."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkActivate(
    [FromBody] BulkActivateLocationsCommand command)
        {
           

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }


        [HttpPatch("bulk/deactivate")]
        //[Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Bulk deactivate locations",
    Description = "Deactivates multiple locations using a list of location IDs."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkDeactivateLocations(
    [FromBody] BulkDeactivateLocationsCommand command)
        {
          

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPatch("bulk/restore")]
        //[Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Bulk restore locations",
    Description = "Restores multiple soft-deleted locations using a list of location IDs."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkRestore(
    [FromBody] BulkRestoreLocationsCommand command)
        {
           
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPatch("bulk/softdelete")]
        //[Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Bulk soft delete locations",
    Description = "Soft deletes multiple locations using a list of location IDs."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkSoftDeleteLocations(
    [FromBody] BulkSoftDeleteLocationsCommand command)
        {
            

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpGet("{id:int}/capacity")]
        [SwaggerOperation(
    Summary = "Get location capacity",
    Description = "Retrieves capacity information for a specific location including total capacity, used capacity, and available space."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCapacity(int id)
        {
           

            var result = await mediator.Send(
                new GetLocationCapacityQuery(id));

            return result.ToActionResult();
        }

        [HttpGet("{id:int}/can-delete")]
        //[Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Check if location can be deleted",
    Description = "Checks whether a specific location can be deleted based on business rules (e.g., no books, no active reservations)."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CanDelete(int id)
        {
         

            var result = await mediator
                .Send(new CanDeleteLocationQuery(id));

            return result.ToActionResult();
        }

        [HttpGet("{id:int}/statistics")]
       // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Get location statistics",
    Description = "Retrieves various statistics for a specific location, such as total books, borrowed books, available space, and active reservations."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLocationStatistics(int id)
        {
           

            var result = await mediator.Send(new GetSpecificLocationStatisticsQuery(id));
            return result.ToActionResult();
        }


        [HttpGet("search")]
        [SwaggerOperation(
    Summary = "Search locations",
    Description = "Searches locations by name, branch, city, and activation status with optional pagination."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchLocations(
    [FromQuery] SearchLocationsQuery query)
        {
            if (query == null)
                return BadRequest("Query cannot be null.");

            var result = await mediator.Send(query);

           
            return result.ToActionResult();
        }


        [HttpGet("statistics")]
      // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
     Summary = "Get locations statistics",
     Description = "Retrieves summary statistics for locations, such as total count, active, inactive, and deleted locations."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStatistics()
        {
            var result = await mediator.Send(new GetLocationStatisticsQuery());
            return result.ToActionResult();
        }


        [HttpDelete("softdelete/{id}")]
        [SwaggerOperation(
     Summary = "Delete an existing location",
     Description = "Delete a location by its Id."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteLocation(
     int id,
     [FromBody] DeleteLocationCommand deleteLocationCommand)
        {
            if (id != deleteLocationCommand.LocationId)
                return BadRequest("Id mismatch.");

            var result = await mediator.Send(deleteLocationCommand);

            return result.ToActionResult();
        }

    }
}
