using LibrarymanagementSystem.Api.Common.Responses;
using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Features.Reviews.Commands.CreateReview;
using LibrarymanagementSystem.Application.Features.Reviews.Commands.CreateReviewReport;
using LibrarymanagementSystem.Application.Features.Reviews.Commands.DeleteReview;
using LibrarymanagementSystem.Application.Features.Reviews.Commands.EditReview;
using LibrarymanagementSystem.Application.Features.Reviews.Dtos;
using LibrarymanagementSystem.Application.Features.Reviews.Queries.CheckUserHasReviewed;
using LibrarymanagementSystem.Application.Features.Reviews.Queries.GetAllReviews;
using LibrarymanagementSystem.Application.Features.Reviews.Queries.GetAllReviewsOfBookToUsers;
using LibrarymanagementSystem.Application.Features.Reviews.Queries.GetAllReviewsWithPagination;
using LibrarymanagementSystem.Application.Features.Reviews.Queries.GetBookRatingSummary;
using LibrarymanagementSystem.Application.Features.Reviews.Queries.GetMyReviewOfSpecificBook;
using LibrarymanagementSystem.Application.Features.Reviews.Queries.GetMyReviews;
using LibrarymanagementSystem.Application.Features.Reviews.Queries.GetMyReviewsWithPagination;
using LibrarymanagementSystem.Application.Features.Reviews.Queries.GetRatingDistribution;
using LibrarymanagementSystem.Application.Features.Reviews.Queries.GetReviewById;
using LibrarymanagementSystem.Application.Features.Reviews.Queries.RestoreReview;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Swashbuckle.AspNetCore.Annotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LibrarymanagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ReviewsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("admin")]
        [SwaggerOperation(
            Summary = "Get all reviews",
            Description = "Retrieves a list of all reviews in the system. Supports pagination if implemented."
        )]
        [ProducesResponseType(typeof(List<AdminReviewDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var result = await mediator.Send(new GetAllReviewsQuery());
            return result.ToActionResult();
        }

        [HttpGet("Admin-pagination")]
        [SwaggerOperation(
       Summary = "Get all reviews with pagination (Admin)",
       Description = "Retrieves paginated reviews for admin purposes. Supports filtering by BookId, UserId, Rating, and soft-deleted reviews. Also supports sorting by any column."
   )]
        [ProducesResponseType(typeof(PaginatedResult<AdminReviewDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllWithPaginationAdmin(
       [FromQuery] int pageNumber = 1,
       [FromQuery] int pageSize = 20,
       [FromQuery] int? bookId = null,
       [FromQuery] string? userId = null,
       [FromQuery] int? rating = null,
       [FromQuery] bool? isDeleted = null,
       [FromQuery] string? orderBy = "CreatedAt",
       [FromQuery] bool orderByDescending = true
   )
        {
            var query = new GetAllReviewsWithPaginationQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                BookId = bookId,
                UserId = userId,
                Rating = rating,
                IsDeleted = isDeleted,
                OrderBy = orderBy,
                OrderByDescending = orderByDescending
            };

            var result = await mediator.Send(query);
            return result.ToActionResult();
        }

        // for admin
        [HttpGet("{id:int}")]
        [SwaggerOperation(
         Summary = "Get review by Id",
         Description = "Retrieves a specific review by its unique Id. Returns review details including rating, comment, and related book information."
     )]
        [ProducesResponseType(typeof(AdminReviewDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await mediator.Send(new GetReviewByIdQuery(id));
            return result.ToActionResult();
        }

        [HttpGet("myreviews")]
        [SwaggerOperation(
     Summary = "Get reviews of the logged-in user",
     Description = "Retrieves all reviews submitted by the currently authenticated user. Requires the user to be logged in."
 )]
        [ProducesResponseType(typeof(List<MyReviewDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMyReviews()
        {
            var result = await mediator.Send(new GetMyReviewsQuery());
            return result.ToActionResult();
        }

        [HttpGet("myreviews/pagination")]
        [SwaggerOperation(
     Summary = "Get paginated reviews of the logged-in user with filtering and sorting",
     Description = "Retrieves paginated reviews submitted by the currently authenticated user. Supports filtering and sorting options."
 )]
        [ProducesResponseType(typeof(PaginatedResult<MyReviewDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyReviewsWithPagination(
   int pageNumber = 1,
    int pageSize = 10,
    ReviewFilter? review = null,
    bool orderByDescending = true)
        {
            var query = new GetMyReviewsWithPaginationQuery(
                pageNumber,
                pageSize,
                review,
                orderByDescending);

            var result = await mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("{bookId}/average-rating")]
        [SwaggerOperation(
    Summary = "Get average rating for a book",
    Description = "Retrieves the average rating and review summary for a specific book by providing the book Id."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BookAverageRate(int bookId)
        {
            var result = await mediator.Send(new GetBookRatingSummaryQuery(bookId));
            return result.ToActionResult();
        }



        [HttpGet("{bookId}/has-reviewed")]
        [SwaggerOperation(
       Summary = "Check if current user has reviewed a book",
       Description = "Checks whether the currently authenticated user has already submitted a review for the specified book by providing the book Id."
   )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] 
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        public async Task<IActionResult> CheckReview(int bookId)
        {
            var result = await mediator.Send(new CheckUserHasReviewedQuery(bookId));
            return result.ToActionResult();
        }


        [HttpPost]
        [SwaggerOperation(
       Summary = "Create a new review",
       Description = "Creates a new review for a specific book by providing rating, comment, and book Id."
   )]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPut("{id:int}")]
        [SwaggerOperation(
      Summary = "Update an existing review",
      Description = "Updates an existing review by Id. The user must be authenticated and can only update their own review. Rating must be between 1 and 5."
  )]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> EditReview([FromBody] EditReviewCommand command, int id)
        {
            if (id != command.Id)
                return BadRequest("Id mismatch");

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpDelete("{id:int}")]
        [SwaggerOperation(
       Summary = "Delete a review",
       Description = "Deletes an existing review by Id. The user must be authenticated and can only delete their own review."
   )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteReview(int id, [FromBody] DeleteReviewCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id mismatch");

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpGet("{bookId:int}/reviews")]
        [SwaggerOperation(
    Summary = "Get all reviews for a specific book",
    Description = "Retrieves all reviews written for a specific book by its Id. Includes rating, comment, and user information."
)]
        [ProducesResponseType(typeof(List<PublicReviewDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetReviewOfBook(int bookId)
        {
            var result = await mediator.Send(new GetAllReviewsOfBookToUsersQuery(bookId));
            return result.ToActionResult();
        }

        [HttpGet("{bookId:int}/myreview")]
        [SwaggerOperation(
     Summary = "Get the logged-in user's review for a specific book",
     Description = "Retrieves the review submitted by the currently authenticated user for a specific book by its Id. Returns the user's rating and comment if a review exists."
 )]
        [ProducesResponseType(typeof(MyReviewDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetmyReviewOfBook(int bookId)
        {
            var result = await mediator.Send(new GetMyReviewOfSpecificBookQuery(bookId));
            return result.ToActionResult();
        }

        [HttpPost("{id}/report")]
        [SwaggerOperation(
      Summary = "Report a review",
      Description = "Allows the currently authenticated user to report a specific review by providing the review Id and report details."
  )]
        [ProducesResponseType(StatusCodes.Status201Created)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)] 
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] 
        public async Task<IActionResult> AddReviewReport(int id, [FromBody] CreateReviewReportCommand createReviewReportCommand)
        {
            if (id != createReviewReportCommand.ReviewId)
                return BadRequest("Id mis match");

            var result = await mediator.Send(createReviewReportCommand);
            return result.ToActionResult();
        }


        [HttpGet("{bookId:int}/rating-distribution")]
        [SwaggerOperation(
     Summary = "Get rating distribution for a book",
     Description = "Retrieves the rating distribution for a specific book by its Id. Returns the number of reviews for each rating value (e.g., 1 to 5 stars)."
 )]
        [ProducesResponseType(typeof(Dictionary<int,int>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        public async Task<IActionResult> GetRateDistribute(int bookId)
        {
            var result = await mediator.Send(new GetRatingDistributionQuery(bookId));
            return result.ToActionResult();
        }
        [HttpPut("{id:int}/restore")]
        [SwaggerOperation(
    Summary = "Restore a soft-deleted review",
    Description = "Restores a previously soft-deleted review by its Id. Only administrators are allowed to perform this action."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RestoreReview(int id)
        {
            var result = await mediator.Send(new RestoreReviewQuery(id));
            return result.ToActionResult();
        }
    }
}
