using LibrarymanagementSystem.Api.Common.Responses;
using LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksByPublisherId;
using LibrarymanagementSystem.Application.Features.Publishers.Commands.AddPublisherLogo;
using LibrarymanagementSystem.Application.Features.Publishers.Commands.BulkCreatePublishers;
using LibrarymanagementSystem.Application.Features.Publishers.Commands.BulkDeletePublishers;
using LibrarymanagementSystem.Application.Features.Publishers.Commands.CreatePublisher;
using LibrarymanagementSystem.Application.Features.Publishers.Commands.DeletePublisher;
using LibrarymanagementSystem.Application.Features.Publishers.Commands.DeletePublisherLogo;
using LibrarymanagementSystem.Application.Features.Publishers.Commands.RestorePublisher;
using LibrarymanagementSystem.Application.Features.Publishers.Commands.UpdatePublisher;
using LibrarymanagementSystem.Application.Features.Publishers.Commands.UpdatePublisherLogo;
using LibrarymanagementSystem.Application.Features.Publishers.Queries.CheckDuplicateName;
using LibrarymanagementSystem.Application.Features.Publishers.Queries.GetAllPublishers;
using LibrarymanagementSystem.Application.Features.Publishers.Queries.GetAllPublisherWithPagination;
using LibrarymanagementSystem.Application.Features.Publishers.Queries.GetDeletedPublishers;
using LibrarymanagementSystem.Application.Features.Publishers.Queries.GetPublisherBooksCount;
using LibrarymanagementSystem.Application.Features.Publishers.Queries.GetPublisherById;
using LibrarymanagementSystem.Application.Features.Publishers.Queries.GetPublisherCount;
using LibrarymanagementSystem.Application.Features.Publishers.Queries.GetPublisherLogo;
using LibrarymanagementSystem.Application.Features.Publishers.Queries.GetPublisherStatistics;
using LibrarymanagementSystem.Application.Features.Publishers.Queries.SearchByName;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LibrarymanagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly IMediator mediator;

        public PublishersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [SwaggerOperation(
     Summary = "Get all publishers",
     Description = "Retrieve all publishers available in the system."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var result = await mediator.Send(new GetAllPublishersQuery());
            return result.ToActionResult();
        }

        [HttpGet("pagination")]
        [SwaggerOperation(
        Summary = "Get publishers with pagination",
        Description = "Retrieve publishers with paging and optional status filtering."
    )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllWithPagination(
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromQuery] PublisherStatusFilter status)
        {
           

            var result = await mediator.Send(new GetAllPublisherWithPaginationQuery(pageNumber, pageSize, status));
            return result.ToActionResult();
        }

        [HttpGet("books/{id}")]
        [SwaggerOperation(
     Summary = "Get books by publisher ID",
     Description = "Retrieve all books that belong to a specific publisher using the publisher's ID."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBooksByPublisherId(int id)
        {
            var result = await mediator.Send(new GetBooksByPublisherIdQuery(id));
            return result.ToActionResult();
        }


        [HttpPut("restore/{id}")]
        [SwaggerOperation(
      Summary = "Restore deleted publisher",
      Description = "Restore a soft deleted publisher using the publisher's ID."
  )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RestorePublish(int id, RestorePublisherCommand restorePublisherCommand)
        {
            if (id != restorePublisherCommand.Id)
                return BadRequest("Id mis match");

            var result = await mediator.Send(restorePublisherCommand);
            return result.ToActionResult();
        }





        [HttpGet("{id}")]
        [SwaggerOperation(
     Summary = "Get a publisher by ID",
     Description = "Retrieve a single publisher from the system using the publisher's ID."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await mediator.Send(new GetPublisherByIdQuery(id));
            return result.ToActionResult();
        }

        [HttpGet("search-by-name")]
        [SwaggerOperation(
      Summary = "Search publishers by name",
      Description = "Retrieve publishers that match the provided name. Partial matches are supported."
  )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByName([FromQuery] string? name,PublisherStatusFilter publisherStatusFilter, [FromQuery] string? country)
        {
            

            var result = await mediator.Send(new SearchByNameQuery(name,publisherStatusFilter,country));
            return result.ToActionResult();
        }

        [HttpGet("{id}/logo")]
        [SwaggerOperation(
     Summary = "Get publisher logo",
     Description = "Retrieves the actual logo image file for the specified publisher. Swagger will show it as an image preview."
 )]
        [Produces("image/png", "image/jpeg", "image/gif")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPublisherLogo(int id)
        {
           
            var result = await mediator.Send(new GetPublisherLogoQuery(id));

            if (!result.IsSuccess || result.Value == null)
                return NotFound(result.Message);

         
            return File(result.Value.Content, result.Value.ContentType);
        }



        [HttpPost]
        [SwaggerOperation(
      Summary = "Add a new publisher",
      Description = "Add a new publisher to the system."
  )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePublisher(CreatePublisherCommand createPublisherCommand)
        {
            var result = await mediator.Send(createPublisherCommand);
            return result.ToActionResult();
        }
        [HttpGet("{id}/statistics")]
        [SwaggerOperation(
            Summary = "Get publisher statistics",
            Description = "Retrieves statistical data for a specific publisher, such as total books, total sales, and average ratings."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPublisherStatistic(int id)
        {
            var result = await mediator.Send(new GetPublisherStatisticsQuery(id));
            return result.ToActionResult();
        }


        [HttpGet("deleted")]
        [SwaggerOperation(
     Summary = "Get deleted publishers",
     Description = "Retrieves all soft-deleted publishers from the system."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDeletedPublisher()
        {
            var result = await mediator.Send(new GetDeletedPublishersQuery());
            return result.ToActionResult();
        }

        [HttpGet("exists")]
        [SwaggerOperation(
     Summary = "Check if publisher name exists",
     Description = "Checks whether a publisher name already exists in the system. Returns true if duplicate exists, otherwise false."
 )]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckDuplicate([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Publisher name is required.");

            var result = await mediator.Send(new CheckDuplicateNameQuery(name));
            return result.ToActionResult();
        }

        [HttpGet("count")]
        [SwaggerOperation(
      Summary = "Get publishers count",
      Description = "Retrieves the total number of publishers in the system."
  )]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> CountPublisher()
        {
            var result = await mediator.Send(new GetPublisherCountQuery());
            return result.ToActionResult();
        }

        [HttpGet("books-count")]
        [SwaggerOperation(
    Summary = "Get publishers books count",
    Description = "Retrieves the total number of books grouped by publisher."
)]
        [ProducesResponseType(typeof(Dictionary<int, int>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPublisherBookCount()
        {
            var result = await mediator.Send(new GetPublisherBooksCountQuery());
            return result.ToActionResult();
        }



        [HttpPut("{id}")]
        [SwaggerOperation(
      Summary = "Update publisher",
      Description = "Update an existing publisher in the system using publisher id."
  )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePublisher(int id, UpdatePublisherCommand updatePublisherCommand)
        {
            if (id != updatePublisherCommand.Id)
                return BadRequest("Id mis match");

            var result = await mediator.Send(updatePublisherCommand);
            return result.ToActionResult();

        }


        [HttpDelete("bulk-delete")]
        [SwaggerOperation(
    Summary = "Bulk delete publishers",
    Description = "Deletes multiple publishers from the system using a list of publisher IDs."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BulkDelete(
    [FromBody] BulkDeletePublishersCommand command)
        {
            

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPost("{id}/logo")]
        [SwaggerOperation(
        Summary = "Add or update a publisher logo",
        Description = "Uploads a logo image for the specified publisher. Accepts a single file in multipart/form-data."
    )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddPublisherLogo(
        int id,
      [FromForm]  AddPublisherLogoCommand addPublisherLogoCommand)
        { 
            var result = await mediator.Send(addPublisherLogoCommand);
            return result.ToActionResult();
        }


        [HttpPost("bulk-add")]
        [SwaggerOperation(
       Summary = "Bulk create publishers",
       Description = "Creates multiple publishers in the system using a list of publisher data."
   )]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkAdd(
       [FromBody] BulkCreatePublishersCommand command)
        {
           

            var result = await mediator.Send(command);

            return result.ToActionResult();
        }


        [HttpPut("{id}/logo")]
        [SwaggerOperation(
        Summary = "Update a publisher logo",
        Description = "Updates the logo image for the specified publisher. Accepts a single file in multipart/form-data."
    )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePublisherLogo(
        int id,
        [FromForm] UpdatePublisherLogoCommand updatePublisherLogoCommand)
        {
            if (id != updatePublisherLogoCommand.Id)
                return BadRequest("Id mismatch.");

            var result = await mediator.Send(updatePublisherLogoCommand);
            return result.ToActionResult();
        }

        [HttpDelete("{id}/logo")]
        [SwaggerOperation(
        Summary = "Delete a publisher logo",
        Description = "Deletes the logo image for the specified publisher."
    )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePublisherLogo(
        int id,
        [FromForm] DeletePublisherLogoCommand deletePublisherLogoCommand)
        {
          
            if (id != deletePublisherLogoCommand.Id)
                return BadRequest("Id mismatch.");

            var result = await mediator.Send(deletePublisherLogoCommand);
            return result.ToActionResult();
        }



        [HttpDelete("{id}")]
        [SwaggerOperation(
     Summary = "Delete a publisher",
     Description = "Delete an existing publisher from the system using the publisher's ID."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePublisher(int id, DeletePublisherCommand deletePublisherCommand)
        {
            if (id != deletePublisherCommand.Id)
                return BadRequest("Id mis match");

            var result = await mediator.Send(deletePublisherCommand);
            return result.ToActionResult();
        }

    }
}
