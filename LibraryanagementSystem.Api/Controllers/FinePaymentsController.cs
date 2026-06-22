using LibrarymanagementSystem.Api.Common.Responses;
using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Features.FinePayments.Commands.ConfirmFinePayment;
using LibrarymanagementSystem.Application.Features.FinePayments.Commands.PayFine;
using LibrarymanagementSystem.Application.Features.FinePayments.Dtos;
using LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetAllFinePayments;
using LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetAllFinePaymentsOfUser;
using LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetAllFinePaymentsWithPagination;
using LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetFinePaymentById;
using LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetFinePayments;
using LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetFinePaymentsStatistics;
using LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetFinePaymentSummary;
using LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetMyPayments;
using LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetMyPaymentSummary;
using LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetRevenuePayments;
using LibrarymanagementSystem.Application.Features.Fines.Commands.AddFine;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics.Contracts;
using System.Security.Claims;

namespace LibrarymanagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinePaymentsController : ControllerBase
    {
        private readonly IMediator mediator;

        public FinePaymentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        [SwaggerOperation(
     Summary = "Get all fine payments",
     Description = "Retrieves a list of all fine payment records in the system. " +
                   "This endpoint returns payment details including paid amount, payment date, and associated fine information."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAll()
        {
            var result = await mediator.Send(new GetAllFinePaymentsQuery());
            return result.ToActionResult();
        }

        [HttpGet("pagination")]
        [SwaggerOperation(
     Summary = "Get fine payments with pagination",
     Description = "Retrieves fine payment records using pagination. " +
                   "Supports page number and page size to efficiently fetch large datasets."
 )]
        [ProducesResponseType(typeof(ApiResponse<PaginatedResult<FinePaymentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetWithPagination(
     [FromQuery] int pageNumber = 1,
     [FromQuery] int pageSize = 10)
        {
            var result = await mediator.Send(
                new GetAllFinePaymentsWithPaginationQuery(pageSize, pageNumber)
            );

            return result.ToActionResult();
        }



        [HttpGet("{id}")]
        [SwaggerOperation(
     Summary = "Get fine payment by Id",
     Description = "Retrieves a specific fine payment record by its unique identifier. " +
                   "Returns detailed information including payment amount, date, and related fine details."
 )]
        [ProducesResponseType(typeof(ApiResponse<FinePaymentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await mediator.Send(new GetFinePaymentByIdQuery(id));
            return result.ToActionResult();
        }

        [HttpGet("payments-of-user/{userId}")]
        [SwaggerOperation(
     Summary = "Get all fine payments of a specific user",
     Description = "Retrieves all fine payments associated with a specific user by their user Id."
 )]
        [ProducesResponseType(typeof(ApiResponse<List<FinePaymentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPaymentsOfUser(string userId)
        {
            var result = await mediator.Send(new GetAllFinePaymentsOfUserQuery(userId));
            return result.ToActionResult();
        }

        [HttpPost]
        [SwaggerOperation(
     Summary = "Pay a fine",
     Description = "Allows a user to pay their library fine. The fine must be outstanding before it can be paid."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PayFine(PayFineCommand command)
        {

            var result = await mediator.Send(command);
            return result.ToActionResult();

        }



        [HttpPost("{finePaymentId}/confirm")]
        [SwaggerOperation(
       Summary = "Confirm a fine payment",
       Description = "Confirms the status of a fine payment made via Card or Online. " +
                     "This endpoint updates the FinePayment status to Completed or Failed based on the payment outcome."
   )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ConfirmFine(int finePaymentId, ConfirmFinePaymentCommand command)
        {
            if (finePaymentId != command.FinePaymentId)
                return BadRequest("Id mismatch");

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }





        [HttpGet("revenue")]
      
        [SwaggerOperation(
            Summary = "Get revenue report",
            Description = "Retrieves total revenue generated from payments within a specified date range. " +
                          "The date range is defined by 'from' and 'to' query parameters."
        )]
        [ProducesResponseType(typeof(RevenueResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetRevenue(
            [FromQuery, SwaggerParameter(Description = "Start date of the report range (inclusive).")] DateTime from,
            [FromQuery, SwaggerParameter(Description = "End date of the report range (inclusive).")] DateTime to)
        {
           

            var result = await mediator.Send(new GetRevenuePaymentsQuery(from, to));
            return result.ToActionResult();
        }
        [HttpGet("{fineId}/payments")]
        [SwaggerOperation(
    Summary = "Get payments for a specific fine",
    Description = "Retrieves all payments that were made for a specific fine using the fine ID."
)]
        [ProducesResponseType(typeof(IEnumerable<FinePaymentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetFinePayments(
    [SwaggerParameter(Description = "The unique identifier of the fine.")]
    int fineId)
        {
            var result = await mediator.Send(new GetFinePaymentsQuery(fineId));

            return result.ToActionResult();
        }

        [HttpGet("{fineId}/payment-summary")]
        [SwaggerOperation(
    Summary = "Get fine payment summary",
    Description = "Retrieves a summary of payments for a specific fine including total paid amount, remaining balance, and payment status."
)]
        [ProducesResponseType(typeof(FinePaymentSummaryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetFinePaymentSummary(
    [SwaggerParameter(Description = "The unique identifier of the fine.")]
    int fineId)
        {
            var result = await mediator.Send(new GetFinePaymentSummaryQuery(fineId));

            return result.ToActionResult();
        }

        [HttpGet("me/payments")]
        [SwaggerOperation(
    Summary = "Get my payments",
    Description = "Retrieves all payments made by the currently authenticated user. The user is identified from the JWT token."
)]
       
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetMyPayments()
        {
            
            var result = await mediator.Send(new GetMyPaymentsQuery());

            return result.ToActionResult();
        }

        [Authorize]
        [HttpGet("me/payment-summary")]
        [SwaggerOperation(
    Summary = "Get my payment summary",
    Description = "Retrieves a summary of all payments made by the currently authenticated user, including total paid amount and number of payments."
)]
       
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetMyPaymentSummary()
        {
           

            var result = await mediator.Send(new GetMyPaymentSummaryQuery());

            return result.ToActionResult();
        }
        //[Authorize(Roles = "Admin,Librarian")]
        [HttpGet("statistics")]
        [SwaggerOperation(
    Summary = "Get fine payments statistics",
    Description = "Retrieves statistical information about fine payments in the system such as total payments count, total collected amount, and average payment value. Accessible only by Admins or Librarians."
)]
        [ProducesResponseType(typeof(FinePaymentsStatisticsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetFinePaymentsStatistics()
        {
            var result = await mediator.Send(new GetFinePaymentsStatisticsQuery());
            return result.ToActionResult();
        }
    }
}
