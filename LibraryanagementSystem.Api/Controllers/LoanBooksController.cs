using LibrarymanagementSystem.Api.Common.Responses;
using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Features.LoanBooks.Commands.BorrowBooks;
using LibrarymanagementSystem.Application.Features.LoanBooks.Dtos;
using LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetAllLateBooks;
using LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetAllLateBooksByUserId;
using LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetAllLoanBooks;
using LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetAllLoanBookWithPagination;
using LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetDamageBooks;
using LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetDamagedBooksWithPagination;
using LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetLoanBookById;
using LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetLoanBooksByUserId;
using LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetLoanBooksStatistics;
using LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetLostBooks;
using LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetLostBooksByUserId;
using LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetLostBooksWithPagination;
using LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetMyDamagedBooks;
using LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetMyLateBooks;
using LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetMyLoanBooks;
using LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetMyLostBooks;
using LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetReturnLoanBooks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LibrarymanagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Manage loaned books: list, pagination, late books, returned books, and get by user or ID.")]
    public class LoanBooksController : ControllerBase
    {
        private readonly IMediator mediator;

        public LoanBooksController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // ===================== GET =====================
        [HttpGet]
        [SwaggerOperation(Summary = "Get all loaned books", Description = "Retrieve all books that are currently loaned.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await mediator.Send(new GetAllLoanBookQuery());
            return result.ToActionResult();
        }

        [HttpGet("pagination")]
        [SwaggerOperation(Summary = "Get loaned books with pagination", Description = "Retrieve loaned books using pagination parameters 'pageNumber' and 'pageSize'.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllWithPagination([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var result = await mediator.Send(new GetAllLoanBookWithPaginationQuery(pageNumber, pageSize));
            return result.ToActionResult();
        }

        [HttpGet("statistics")]
       
        [SwaggerOperation(
     Summary = "Get loan books statistics",
     Description = "Retrieves various statistics about loaned books in the system, " +
                   "such as total loans, late books, lost books, and damaged books. " +
                   "Accessible only to administrators or librarians."
 )]
        [ProducesResponseType(typeof(LibraryDashboardDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetStatistics()
        {
            var result = await mediator.Send(new GetLoanBooksStatisticsQuery());
            return result.ToActionResult();
        }

        [HttpGet("me/loan-books")]
       
        [SwaggerOperation(
     Summary = "Get my loaned books",
     Description = "Retrieves all books currently loaned by the authenticated user. " +
                   "Requires a valid JWT token in the Authorization header."
 )]
        [ProducesResponseType(typeof(List<LoanBookDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyLoanBooks()
        {
            var result = await mediator.Send(new GetMyLoanBooksQuery());
            return result.ToActionResult();
        }



        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get loaned book by ID", Description = "Retrieve a specific loaned book by its ID.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await mediator.Send(new GetLoanBookByIdQuery(id));
            return result.ToActionResult();
        }

        [HttpGet("lostbooks")]
        [SwaggerOperation(
     Summary = "Get all lost books",
     Description = "Retrieve a list of all books that have been reported as lost."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLostBooks()
        {
            var result = await mediator.Send(new GetLostBooksQuery());

          
            if (result.Value == null || !result.Value.Any())
                return NotFound(new { Message = "No lost books found." });

            return result.ToActionResult();
        }


        [HttpGet("lostbooks-pagination")]
        [SwaggerOperation(
     Summary = "Get lost books with pagination",
     Description = "Retrieve a paginated list of books that have been reported as lost."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLostBooksWithPagination(int PageNumber = 1, int PageSize = 10)
        {
          
            var result = await mediator.Send(new GetLostBooksWithPaginationQuery(PageNumber, PageSize));

           

            return result.ToActionResult();
        }
        [HttpGet("damagedbooks")]
        [SwaggerOperation(
            Summary = "Get all damaged books",
            Description = "Retrieves a list of all books that have been marked as damaged in the system. " +
                          "This endpoint is typically restricted to administrators or librarians."
        )]
        [ProducesResponseType(typeof(List<DamageBookDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetDamagedBooks()
        {
            var result = await mediator.Send(new GetDamageBooksQuery());
            return result.ToActionResult();
        }



        [HttpGet("damagedbooks-pagination")]
        [SwaggerOperation(
      Summary = "Get damaged books with pagination",
      Description = "Retrieves a paginated list of books marked as damaged. " +
                    "Supports pagination using pageNumber and pageSize query parameters."
  )]
        [ProducesResponseType(typeof(PaginatedResult<DamageBookDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetDamagedBooks(
      [FromQuery, SwaggerParameter(Description = "Page number (starting from 1).")] int pageNumber = 1,
      [FromQuery, SwaggerParameter(Description = "Number of records per page.")] int pageSize = 10)
        {
            var result = await mediator.Send(
                new GetDamagedBooksWithPaginationQuery(pageNumber, pageSize));

            return result.ToActionResult();
        }


        [HttpGet("lostbooks/{userId}")]
        [SwaggerOperation(
      Summary = "Get lost books by user ID",
      Description = "Retrieve a list of books reported as lost by a specific user."
  )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLostBooksByUser(string userId)
        {
           

            var result = await mediator.Send(new GetLateBooksByUserIdQuery(userId));

          
            return result.ToActionResult();
        }
        [HttpGet("me/lostbooks")]
        [SwaggerOperation(
    Summary = "Get my lost books",
    Description = "Retrieves all books that have been marked as lost by the currently authenticated user. " +
                  "Requires a valid JWT token in the Authorization header."
)]
        [ProducesResponseType(typeof(List<DamageBookDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMyLostBooks()
        {
            var result = await mediator.Send(new GetMyLostBooksQuery());
            return result.ToActionResult();
        }






        [HttpGet("me/damagedbooks")]
        [Authorize]
        [SwaggerOperation(
      Summary = "Get my damaged books",
      Description = "Retrieves all books marked as damaged by the currently authenticated user. " +
                    "Requires a valid JWT token in the Authorization header."
  )]
        [ProducesResponseType(typeof(List<DamageBookDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyDamagedBooks()
        {
            var result = await mediator.Send(new GetMyDamagedBooksQuery());
            return result.ToActionResult();
        }


        [HttpGet("me/late-books")]
        [Authorize]
        [SwaggerOperation(
    Summary = "Get my late books",
    Description = "Retrieves all overdue (late) books for the currently authenticated user. " +
                  "A book is considered late if its due date has passed and it has not been returned. " +
                  "Requires a valid JWT token in the Authorization header."
)]
        [ProducesResponseType(typeof(List<LateBookDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyLateBooks()
        {
            var result = await mediator.Send(new GetMyLateBooksQuery());
            return result.ToActionResult();
        }

        [HttpGet("latebooks/{userId}")]
      
        [SwaggerOperation(
    Summary = "Get late books by user ID",
    Description = "Retrieves all overdue (late) books for the specified user. " +
                  "A book is considered late if the due date has passed and it has not been returned."
)]
        [ProducesResponseType(typeof(List<LateBookDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLateBooksByUserId(
    [FromRoute, SwaggerParameter(Description = "The unique identifier of the user.")]
    string userId)
        {
            var result = await mediator.Send(new GetAllLateBooksByUserIdQuery(userId));
            return result.ToActionResult();
        }

        [HttpGet("loanbooks/{userId}")]
       
        [SwaggerOperation(
     Summary = "Get loaned books by user ID",
     Description = "Retrieves all books currently loaned by the specified user. " +
                   "Accessible only to administrators or librarians."
 )]
        [ProducesResponseType(typeof(List<LoanBookDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLoanBooksByUserId(
     [FromRoute, SwaggerParameter(Description = "The unique identifier of the user.")]
    string userId)
        {
            var result = await mediator.Send(new GetLoanBooksByUserIdQuery(userId));
            return result.ToActionResult();
        }



        [HttpGet("lateBooks")]
        [SwaggerOperation(Summary = "Get late books", Description = "Retrieve all books that are past their return date.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLateBooks()
        {
            var result = await mediator.Send(new GetAllLateBooksQuery());
            return result.ToActionResult();
        }

        [HttpGet("returnedBooks")]
        [SwaggerOperation(Summary = "Get returned books", Description = "Retrieve all books that have been returned.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReturnedBooks()
        {
            var result = await mediator.Send(new GetReturnLoanBookQuery());
            return result.ToActionResult();
        }
    }

}
