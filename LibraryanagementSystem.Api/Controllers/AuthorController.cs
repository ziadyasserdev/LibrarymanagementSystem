using LibrarymanagementSystem.Api.Common.Responses;
using LibrarymanagementSystem.Application.Features.Authors.Commands.ActiveAuthor;
using LibrarymanagementSystem.Application.Features.Authors.Commands.BulkActiveAuthor;
using LibrarymanagementSystem.Application.Features.Authors.Commands.BulkDeactiveAuthor;
using LibrarymanagementSystem.Application.Features.Authors.Commands.BulkDeleteAuthor;
using LibrarymanagementSystem.Application.Features.Authors.Commands.BulkRestoreAuthor;
using LibrarymanagementSystem.Application.Features.Authors.Commands.CreateAuthor;
using LibrarymanagementSystem.Application.Features.Authors.Commands.DeactiveAuthor;
using LibrarymanagementSystem.Application.Features.Authors.Commands.DeleteAuthor;
using LibrarymanagementSystem.Application.Features.Authors.Commands.RestoreAuthor;
using LibrarymanagementSystem.Application.Features.Authors.Commands.UpdateAuthor;
using LibrarymanagementSystem.Application.Features.Authors.Queries.CanDeleteAuthor;
using LibrarymanagementSystem.Application.Features.Authors.Queries.CheckAuthorExistsByName;
using LibrarymanagementSystem.Application.Features.Authors.Queries.GetAllAuthors;
using LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorById;
using LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorByName;
using LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorCount;
using LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorStatistics;
using LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorsWithPagination;
using LibrarymanagementSystem.Application.Features.Authors.Queries.SearchAuthors;
using LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksByAuthorId;
using LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksByAuthorName;
using LibrarymanagementSystem.Application.Features.Categories.Queries.GetCategoriesByAuthorId;
using LibrarymanagementSystem.Application.Features.Categories.Queries.GetCategoriesByAuthorName;
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
    [SwaggerTag("Manage authors: create, update, delete, list, search, pagination, and get books by author.")]
    public class AuthorController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthorController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // ===================== POST =====================
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new author", Description = "Add a new author to the system.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAuthor(CreateAuthorCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        // ===================== PUT =====================
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update author", Description = "Update an existing author using their ID.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAuthor(int id, UpdateAuthorCommand command)
        {
            if (id != command.AuthorId)
                return BadRequest("ID mismatch");

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        // ===================== DELETE =====================
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete author", Description = "Delete an author using their ID.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var result = await mediator.Send(new DeleteAuthorCommand(id));
            return result.ToActionResult();
        }

        // ===================== GET =====================
        [HttpGet]
        [SwaggerOperation(Summary = "Get all authors", Description = "Retrieve all authors in the system.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAuthors()
        {
            var result = await mediator.Send(new GetAllAuthorQuery());
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get author by ID", Description = "Retrieve a specific author by their ID.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await mediator.Send(new GetAuthorByIdQuery(id));
            return result.ToActionResult();
        }

        [HttpGet("By-Name")]
        [SwaggerOperation(Summary = "Get author by name", Description = "Retrieve an author using their name.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuthorByName([FromQuery] string name)
        {
            var result = await mediator.Send(new GetAuthorByNameQuery(name.ToLower()));
            return result.ToActionResult();
        }

        [HttpGet("{authorName:}/books")]
        [SwaggerOperation(Summary = "Get books by author name", Description = "Retrieve all books written by a specific author.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBookByAuthorName(string authorName)
        {
            var result = await mediator.Send(new GetBooksByAuthorNameQuery(authorName.ToLower()));
            return result.ToActionResult();
        }
        [HttpGet("{authorName:}/categories")]

        public async Task<IActionResult> GetCategoriesByAuthorName(string authorName)
        {
            var result = await mediator.Send(new GetCategoriesByAuthorNameQuery(authorName.ToLower()));
            return result.ToActionResult();
        }




        [HttpGet("{authorId:int}/books")]
        [SwaggerOperation(
     Summary = "Get books by author Id",
     Description = "Retrieve all books written by a specific author using the author Id."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBookByAuthorId(int authorId)
        {
            var result = await mediator.Send(new GetBooksByAuthorIdQuery(authorId));
            return result.ToActionResult();
        }

        [HttpGet("{authorId:int}/categories")]
        [SwaggerOperation(
      Summary = "Get categories by author Id",
      Description = "Retrieve all categories that contain books written by a specific author using the author Id."
  )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCategoriesByAuthorId(int authorId)
        {
            var result = await mediator.Send(new GetCategoriesByAuthorIdQuery(authorId));
            return result.ToActionResult();
        }


        [HttpGet("pagination")]
        [SwaggerOperation(Summary = "Get authors with pagination", Description = "Retrieve authors using pagination parameters 'pageNumber' and 'pageSize'.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuthorsWithPagination([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var result = await mediator.Send(new GetAuthorsWithPaginationQuery(pageNumber, pageSize));
            return result.ToActionResult();
        }
        [HttpPatch("{id:int}/restore")]
        //[Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Restore an author",
    Description = "Restores a soft-deleted author by its Id."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RestoreAuthor(int id)
        {


            var result = await mediator.Send(new RestoreAuthorCommand(id));
            return result.ToActionResult();
        }

        [HttpPatch("{id:int}/activate")]
        // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Activate an author",
    Description = "Activates an existing author by its Id."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ActivateAuthor(int id)
        {


            var result = await mediator.Send(new ActiveAuthorCommand(id));
            return result.ToActionResult();
        }
        [HttpPatch("{id:int}/deactivate")]
        // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Deactivate an author",
    Description = "Deactivates an existing author by its Id."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeactivateAuthor(int id)
        {


            var result = await mediator.Send(new DeactiveAuthorCommand(id));
            return result.ToActionResult();
        }
        [HttpPatch("bulk/activate")]
        // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Bulk activate authors",
    Description = "Activates multiple authors by their Ids."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BulkActivateAuthors(BulkActiveAuthorCommnand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPatch("bulk/deactivate")]
        // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Bulk deactivate authors",
    Description = "Deactivates multiple authors by their Ids."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BulkDeactivateAuthors(BulkDeactiveAuthorCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
    
    [HttpDelete("bulk")]
        // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Bulk delete authors",
    Description = "Deletes multiple authors by their Ids."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteAuthorCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPatch("bulk/restore")]
        // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Bulk restore authors",
    Description = "Restores multiple deleted authors by their Ids."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BulkRestore([FromBody] BulkRestoreAuthorCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpGet("count")]
        // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Get authors count",
    Description = "Returns the total number of authors. Optionally filter by deletion status using the isDeleted query parameter."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAuthorCount([FromQuery] bool? isDeleted)
        {
            var result = await mediator.Send(new GetAuthorCountQuery(isDeleted));
            return result.ToActionResult();
        }

        [HttpGet("exists")]
        // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Check if an author exists by name",
    Description = "Checks whether an author exists in the system by providing the author's name as a query parameter."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CheckAuthorExists([FromQuery] string name)
        {
           

            var result = await mediator.Send(new CheckAuthorExistsByNameQuery(name.Trim()));
            return result.ToActionResult();
        }

        [HttpGet("{authorId:int}/can-delete")]
        // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Check if an author can be deleted",
    Description = "Checks whether an author can be deleted. Authors with associated books or already deleted cannot be deleted."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CanDeleteAuthor(int authorId)
        {
           
            var result = await mediator.Send(new CanDeleteAuthorQuery(authorId));
            return result.ToActionResult();
        }

        [HttpGet("{id}/statistics")]
        // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
       Summary = "Get author statistics",
       Description = "Retrieves statistics for a specific author, including total books, active books, borrowed books, and other metrics."
   )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAuthorStatistics(int id)
        {
          

            var query = new GetAuthorStatisticsQuery(id);
            var result = await mediator.Send(query);

            return result.ToActionResult();
        }

        [HttpGet("search")]
        // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Search authors with pagination and sorting",
    Description = "Searches authors by name or other criteria. Supports pagination, sorting, and optional inclusion of deleted authors."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchAuthors(
    [FromQuery] string? searchTerm,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] AuthorSort authorSort = AuthorSort.Name,
    [FromQuery] bool descending = false,
    [FromQuery] bool? includeDeleted = null)
        {
          

           
            var query = new SearchAuthorsQuery
            {
                SearchTerm = searchTerm,
                PageNumber = pageNumber,
                PageSize = pageSize,
                AuthorSort = authorSort,
                Descending = descending,
                IncludeDeleted = includeDeleted
            };

           
            var result = await mediator.Send(query);

          
            return result.ToActionResult();
        }
    }
}
