using LibrarymanagementSystem.Api.Common.Responses;
using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.BookCopies.Commands.AddBookCopy;
using LibrarymanagementSystem.Application.Features.BookCopies.Commands.BulkDeleteBookCopies;
using LibrarymanagementSystem.Application.Features.BookCopies.Commands.BulkRestoreBookCopies;
using LibrarymanagementSystem.Application.Features.BookCopies.Commands.DeleteBookCopy;
using LibrarymanagementSystem.Application.Features.BookCopies.Commands.EditBookCopy;
using LibrarymanagementSystem.Application.Features.BookCopies.Commands.RestoreBookCopy;
using LibrarymanagementSystem.Application.Features.BookCopies.Dtos;
using LibrarymanagementSystem.Application.Features.BookCopies.Queries.CanDeleteBookCopy;
using LibrarymanagementSystem.Application.Features.BookCopies.Queries.GetAllCopies;
using LibrarymanagementSystem.Application.Features.BookCopies.Queries.GetAvailableBookCopies;
using LibrarymanagementSystem.Application.Features.BookCopies.Queries.GetBookCopiesByBook;
using LibrarymanagementSystem.Application.Features.BookCopies.Queries.GetBookCopiesCount;
using LibrarymanagementSystem.Application.Features.BookCopies.Queries.GetBookCopiesStatistics;
using LibrarymanagementSystem.Application.Features.BookCopies.Queries.GetBookCopiesWithPagination;
using LibrarymanagementSystem.Application.Features.BookCopies.Queries.GetBookCopy;
using LibrarymanagementSystem.Application.Features.BookCopies.Queries.SearchBookCopies;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LibrarymanagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookCopiesController : ControllerBase
    {
        private readonly IMediator mediator;

        public BookCopiesController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Add a copy of a book",
    Description = "Adds a new copy of an existing book to the library. The command must include the BookId and the number of copies to add."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddBookCopy([FromBody] AddBookCopyCommand command)
        {
         

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Edit a book copy",
    Description = "Updates an existing book copy details including barcode, book reference, and location. Editing is not allowed if the copy is lost or currently loaned."
)]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Edit(int id, [FromBody] EditBookCopyCommand command)
        {
            command.Id = id;

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Delete a book copy",
    Description = "Soft deletes a book copy. Deletion is not allowed if the copy is currently loaned or lost."
)]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteBookCopy(int id)
        {
            var result = await mediator.Send(new DeleteBookCopyCommand(id));
            return result.ToActionResult();
        }

        [HttpGet]
      
        [SwaggerOperation(
           Summary = "Get all book copies",
           Description = "Returns all book copies. Projection depends on user role (Admin or User)."
       )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            
          

            var result = await mediator.Send(new GetAllCopiesQuery ());
            return result.ToActionResult();
        }

        [HttpGet("{id:int}")]
        [SwaggerOperation(
    Summary = "Get book copy by Id",
    Description = "Returns a specific book copy by its Id. Projection depends on user role (Admin or User)."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await mediator.Send(new GetBookCopyQuery(id));

            return result.ToActionResult();
        }

        [HttpGet("pagination")]
        [SwaggerOperation(
         Summary = "Get book copies with pagination",
         Description = "Returns a paginated list of book copies. Admins see full details, users see limited info."
     )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookCopiesWithPagination(
         [FromQuery] int pageNumber = 1,
         [FromQuery] int pageSize = 10)
        {
           

            var query = new GetBookCopiesWithPaginationQuery(pageNumber, pageSize);

            var result = await mediator.Send(query);

            return result.ToActionResult();
        }

        [HttpPatch("{id:int}/restore")]
       // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Restore a deleted book copy",
    Description = "Restores a previously deleted book copy. Only users with the Admin role can perform this action."
)]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Restore(int id)
        {
            var result = await mediator.Send(new RestoreBookCopyCommand(id));
            return result.ToActionResult();
        }

        [HttpDelete("bulk")]
       // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Bulk delete book copies",
    Description = "Soft deletes multiple book copies by their Ids. Only users with the Admin role can perform this action."
)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteBookCopiesCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPatch("bulk/restore")]
       // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Bulk restore book copies",
    Description = "Restores multiple previously deleted book copies by their Ids. Only users with the Admin role can perform this action."
)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> BulkRestore([FromBody] BulkRestoreBookCopiesCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpGet("count")]
        //[Authorize(Roles = "Admin")]
        [SwaggerOperation(
    Summary = "Get book copies counts",
    Description = "Returns the total count of book copies, including active, deleted, and optionally other statuses. Only Admin users can access this endpoint."
)]
        [ProducesResponseType(typeof(Result<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCounts()
        {
            var result = await mediator.Send(new GetBookCopiesCountQuery());
            return result.ToActionResult();
        }
        [HttpGet("search")]
        [SwaggerOperation(
           Summary = "Search book copies",
           Description = "Search book copies by book, location, branch, or free text. Projection depends on user role (Admin or User)."
       )]
        [ProducesResponseType(typeof(Result<PaginatedResult<object>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Search([FromQuery] SearchBookCopiesQuery query)
        {
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }
        [HttpGet("statistics")]
        [SwaggerOperation(
    Summary = "Get book copies statistics",
    Description = "Returns statistics of book copies filtered by book, branch, or location."
)]
        [ProducesResponseType(typeof(Result<BookCopyStatisticsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetStatistics([FromQuery] GetBookCopiesStatisticsQuery query)
        {
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("{id}/can-delete")]
        [SwaggerOperation(
    Summary = "Check if a book copy can be deleted",
    Description = "Checks whether a specific book copy can be safely deleted or if it is linked to active operations such as loans."
)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CanDeleteBookCopy(int id)
        {
            var result = await mediator.Send(new CanDeleteBookCopyQuery(id));
            return result.ToActionResult();
        }

      //  [Authorize(Roles = "Admin,Librarian")]
        [HttpGet("{bookId}/copies")]
        [SwaggerOperation(
    Summary = "Get paginated copies of a specific book (Admin view)",
    Description = "Returns all copies with pagination, filtering, and sorting for Admins/Librarians."
)]
        [ProducesResponseType(typeof(Result<PaginatedResult<BookCopyAdminDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookCopiesForAdmin(int bookId,[FromQuery] GetBookCopiesByBookQuery getBookCopiesByBookQuery)
        {
            if(bookId != getBookCopiesByBookQuery.BookId)
                return BadRequest("BookId in route does not match BookId in query parameters");
            var result = await mediator.Send(getBookCopiesByBookQuery);
            return result.ToActionResult();
        }
        [HttpGet("{bookId}/available-copies")]
        [ProducesResponseType(typeof(Result<PaginatedResult<UserBookCopyDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
         Summary = "Get available copies of a book (User view)",
         Description = "Returns the available copies for a book including branch and location information for Users."
     )]
        public async Task<IActionResult> GetAvailableBookCopies([FromQuery] GetAvailableBookCopiesQuery query,int bookId)
        {
            if(bookId != query.BookId)
                return BadRequest("BookId in route does not match BookId in query parameters");
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }
    }
}
