



using LibrarymanagementSystem.Api.Common.Responses;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Books.Commands.ActivateBook;
using LibrarymanagementSystem.Application.Features.Books.Commands.BulkActivateBooks;
using LibrarymanagementSystem.Application.Features.Books.Commands.BulkDeactivateBooks;
using LibrarymanagementSystem.Application.Features.Books.Commands.BulkDeleteBooks;
using LibrarymanagementSystem.Application.Features.Books.Commands.BulkRestoreBooks;
using LibrarymanagementSystem.Application.Features.Books.Commands.CreateBook;
using LibrarymanagementSystem.Application.Features.Books.Commands.CreateBookFile;
using LibrarymanagementSystem.Application.Features.Books.Commands.DeactivateBook;
using LibrarymanagementSystem.Application.Features.Books.Commands.DeleteBook;
using LibrarymanagementSystem.Application.Features.Books.Commands.DeleteBookFile;
using LibrarymanagementSystem.Application.Features.Books.Commands.RestoreBook;
using LibrarymanagementSystem.Application.Features.Books.Commands.UpdateBook;
using LibrarymanagementSystem.Application.Features.Books.Commands.UpdateBookFile;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using LibrarymanagementSystem.Application.Features.Books.Queries.CanDeleteBook;
using LibrarymanagementSystem.Application.Features.Books.Queries.DownloadBookFile;
using LibrarymanagementSystem.Application.Features.Books.Queries.GetAdvancedBookStatistics;
using LibrarymanagementSystem.Application.Features.Books.Queries.GetAllAvailableBooks;
using LibrarymanagementSystem.Application.Features.Books.Queries.GetAllBooks;
using LibrarymanagementSystem.Application.Features.Books.Queries.GetBookById;
using LibrarymanagementSystem.Application.Features.Books.Queries.GetBookByName;
using LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksCount;
using LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksWithPagination;
using LibrarymanagementSystem.Application.Features.Books.Queries.SearchBooks;
using LibrarymanagementSystem.Application.Features.LoanBooks.Commands.BorrowBooks;
using LibrarymanagementSystem.Application.Features.LoanBooks.Commands.ReturnBooks;
using LibrarymanagementSystem.Application.Features.Reviews.Dtos;
using LibrarymanagementSystem.Application.Features.Reviews.Queries.GetAllReviewsOfBookToUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;

namespace LibrarymanagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Manage books: add, update, delete, borrow, return, list, download, and pagination.")]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }
        //[Authorize]
        // GET: api/books
        [EnableRateLimiting("Fixed")]
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all books",
            Description = "Retrieve a list of all books in the system."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllBookQuery());
            return result.ToActionResult();
        }

        // GET: api/books/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get book by ID",
            Description = "Retrieve a specific book by its unique ID."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetBookByIdQuery(id));
            return result.ToActionResult();
        }

     

       
        [HttpGet("pagination")]
        [SwaggerOperation(
            Summary = "Get books with pagination",
            Description = "Retrieve books using pagination parameters 'pageNumber' and 'pageSize'."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBooksWithPagination(int pageNumber, int pageSize)
        {
            var result = await _mediator.Send(new GetBooksWithPaginationQuery(pageNumber, pageSize));
            return result.ToActionResult();
        }

        // POST: api/books
        [HttpPost]
        [SwaggerOperation(
            Summary = "Add a new book",
            Description = "Add a new book to the system."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddBook(CreateBookCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        // POST: api/books/borrow/{bookId}
        [HttpPost("borrow/{bookCopyId}")]
        [SwaggerOperation(
            Summary = "Borrow a book",
            Description = "Borrow a book for the authenticated user."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BorrowBook(int bookCopyId)
        {
            var result = await _mediator.Send(new BorrowBookCommand { BookCopy = bookCopyId });
            return result.ToActionResult();
        }

        // POST: api/books/return/{bookId}
        [HttpPost("return/{bookId}")]
        [SwaggerOperation(
            Summary = "Return a borrowed book",
            Description = "Return a previously borrowed book."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReturnBook(int bookId)
        {
            var result = await _mediator.Send(new ReturnBookCommand(bookId));
            return result.ToActionResult();
        }

        // PUT: api/books/{id}
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update book",
            Description = "Update the details of an existing book."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBook(int id, UpdateBookCommand command)
        {
            if (id != command.Id) return BadRequest("Book Id mismatch.");
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }



        // DELETE: api/books/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(
     Summary = "Soft delete a book",
     Description = "Marks the book as deleted without removing it from the database."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result = await _mediator.Send(new DeleteBookCommand(id));
            return result.ToActionResult();
        }

        // POST: api/books/add-book-file
        [HttpPost("add-book-file")]
        [SwaggerOperation(
            Summary = "Add a book file",
            Description = "Upload a file for a book."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddBookFile([FromForm] CreateBookFileCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        // PUT: api/books/update-book-file
        [HttpPut("update-book-file")]
        [SwaggerOperation(
            Summary = "Update a book file",
            Description = "Update the uploaded file of a book."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBookFile([FromForm] UpdateBookFileCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        // DELETE: api/books/delete-book-file
        [HttpDelete("delete-book-file")]
        [SwaggerOperation(
            Summary = "Delete a book file",
            Description = "Delete the uploaded file of a book."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteBookFile(int bookId)
        {
            var result = await _mediator.Send(new DeleteBookFileCommand(bookId));
            return result.ToActionResult();
        }

        // GET: api/books/download-book-file
        [HttpGet("download-book-file")]
        [SwaggerOperation(
            Summary = "Download a book file",
            Description = "Download the uploaded file of a book by its ID."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadBookFile(int bookId)
        {
            var result = await _mediator.Send(new DownloadBookFileQuery(bookId));
            return File(result.Value.Content, result.Value.ContentType, result.Value.FileName);
        }

        [HttpPatch("{id}/activate")]
        [SwaggerOperation(
    Summary = "Activate a book",
    Description = "Activates a specific book by its ID."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Activate(int id)
        {
            var result = await _mediator.Send(new ActivateBookCommand { Id = id });
            return result.ToActionResult();
        }


        [HttpPatch("{id}/deactivate")]
        [SwaggerOperation(
    Summary = "Deactivate a book",
    Description = "Deactivates a specific book by setting its IsActive flag to false."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await _mediator.Send(new DeactivateBookCommand(id));
            return result.ToActionResult();
        }

        [HttpPatch("{id}/restore")]
        [SwaggerOperation(
    Summary = "Restore a soft deleted book",
    Description = "Restores a previously soft deleted book by setting its IsDeleted flag to false."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Restore(int id)
        {
            var result = await _mediator.Send(new RestoreBookCommand(id));
            return result.ToActionResult();
        }

        [HttpPatch("bulk/activate")]
        [SwaggerOperation(
    Summary = "Bulk activate books",
    Description = "Activates multiple books by their IDs in a single request."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkActivate([FromBody] List<int> bookIds)
        {
           

            var result = await _mediator.Send(new BulkActivateBooksCommand(bookIds));
            return result.ToActionResult();
        }

        [HttpPatch("bulk/deactivate")]
        [SwaggerOperation(
    Summary = "Bulk deactivate books",
    Description = "Deactivates multiple books by their IDs in a single request."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkDeactivate([FromBody] List<int> bookIds)
        {
          

            var result = await _mediator.Send(new BulkDeactivateBooksCommand(bookIds));
            return result.ToActionResult();
        }
        [HttpPatch("bulk/restore")]
        [SwaggerOperation(
    Summary = "Bulk restore soft deleted books",
    Description = "Restores multiple books that were previously soft deleted by their IDs."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkRestore([FromBody] List<int> bookIds)
        {
            

            var result = await _mediator.Send(new BulkRestoreBooksCommand(bookIds));
            return result.ToActionResult();
        }

        [HttpDelete("bulk")]
        [SwaggerOperation(
    Summary = "Bulk soft delete books",
    Description = "Performs a soft delete on multiple books by marking them as deleted without removing them from the database."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkDelete([FromBody] List<int> bookIds)
        {
            

            var command = new BulkDeleteBooksCommand(bookIds);

            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpGet("search")]
        [SwaggerOperation(
    Summary = "Search books",
    Description = "Search for books based on title, author, category, or other filters."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchBooks([FromQuery] SearchBooksQuery query)
        {
          
            var result = await _mediator.Send(query);

            return result.ToActionResult();
        }

        [HttpGet("statistics/advanced")]
        [SwaggerOperation(
    Summary = "Get advanced book statistics",
    Description = "Returns detailed book statistics including counts, averages, top 5 metrics, and aggregations per category, author, and publisher."
)]
        [ProducesResponseType(typeof(Result<BookStatisticsDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAdvancedStatistics()
        {
            var result = await _mediator.Send(new GetAdvancedBookStatisticsQuery());
            return result.ToActionResult();
        }

        [HttpGet("{id}/can-delete")]
        public async Task<IActionResult> CanDeleteBook(int id)
        {
            var query = new CanDeleteBookQuery(id);

           
            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }
        [HttpGet("count")]
        [SwaggerOperation(
    Summary = "Get total number of books",
    Description = "Returns the total number of books in the system. Can include soft deleted books with the query parameter."
)]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBooksCount([FromQuery] bool? includeDeleted = false)
        {
            var query = new GetBooksCountQuery { IncludeDeleted = includeDeleted };
            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

    }

}