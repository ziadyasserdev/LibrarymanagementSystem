using Hangfire;
using LibrarymanagementSystem.Api.Common.Responses;
using LibrarymanagementSystem.Application.BackgroundJobs.Fines;
using LibrarymanagementSystem.Application.Features.Fines.Commands.AddFine;
using LibrarymanagementSystem.Application.Features.Fines.Commands.DeleteFine;
using LibrarymanagementSystem.Application.Features.Fines.Commands.UpdateFine;
using LibrarymanagementSystem.Application.Features.Fines.Queries.GetAllFines;
using LibrarymanagementSystem.Application.Features.Fines.Queries.GetAllFinesByUserId;
using LibrarymanagementSystem.Application.Features.Fines.Queries.GetAllFinesOfCurrentUser;
using LibrarymanagementSystem.Application.Features.Fines.Queries.GetAllFinesWithPagination;
using LibrarymanagementSystem.Application.Features.Fines.Queries.GetAllPaidFines;
using LibrarymanagementSystem.Application.Features.Fines.Queries.GetAllUnpaidFines;
using LibrarymanagementSystem.Application.Features.Fines.Queries.GetFineById;
using LibrarymanagementSystem.Application.Features.Fines.Queries.GetFineRemaining;
using LibrarymanagementSystem.Application.Features.Fines.Queries.GetFinesByDate;
using LibrarymanagementSystem.Application.Features.Fines.Queries.GetFinesReport;
using LibrarymanagementSystem.Application.Features.Fines.Queries.GetFineStats;
using LibrarymanagementSystem.Application.Features.Fines.Queries.GetOverdueFines;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LibrarymanagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinesController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IFineJobs fineJobs;

        public FinesController(IMediator mediator,IFineJobs fineJobs)
        {
            this.mediator = mediator;
            this.fineJobs = fineJobs;
        }
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all fines",
            Description = "Retrieve a list of all fines in the system including paid and unpaid fines."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var result = await mediator.Send(new GetAllFinesQuery());
            return result.ToActionResult();
        }
        [HttpGet("{id}")]
        [SwaggerOperation(
    Summary = "Get fine by Id",
    Description = "Retrieve a specific fine by its unique identifier. Returns fine details including amount, status, and related loan information."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await mediator.Send(new GetFineByIdQuery(id));
            return result.ToActionResult();
        }

        [HttpGet("user/{id:guid}")]
        [SwaggerOperation(
    Summary = "Get all fines for a user",
    Description = "Retrieves all fines associated with a specific user by their ID. Returns a list of fines or indicates if none exist."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllFinesByUserId(string id)
        {
            var result = await mediator.Send(new GetAllFinesByUserIdQuery(id));
            return result.ToActionResult();
        }

        [HttpGet("my")]
        [SwaggerOperation(
     Summary = "Get current user's fines",
     Description = "Retrieves all fines associated with the currently authenticated user."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllFinesOfCurrentUser(FineStatus fineStatus)
        {
            var result = await mediator.Send(new GetAllFinesOfCurrentUserQuery(fineStatus));
            return result.ToActionResult();
        }

        [HttpGet("pagination")]
        [SwaggerOperation(
     Summary = "Get all fines with pagination",
     Description = "Retrieve a paginated list of fines in the system. Supports page number and page size parameters."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllFinesWithPagination(int PageNumber, int PageSize)
        {
            var result = await mediator.Send(
                new GetAllFinesWithPaginationQuery(PageNumber, PageSize));

            return result.ToActionResult();
        }

        [HttpPost]
        [SwaggerOperation(
      Summary = "Add a new fine",
      Description = "Creates a new fine for a loaned book. The fine can be related to overdue books or other penalties."
  )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddFine(AddFineCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
     Summary = "Update fine",
     Description = "Updates an existing fine by its Id. Allows modifying fine amount or related details if applicable."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateFine(int id, UpdateFineCommand command)
        {
            if (id != command.Id)
                return BadRequest("Route Id does not match body Id.");

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpDelete("{id}")]
        [SwaggerOperation(
    Summary = "Delete fine",
    Description = "Deletes an existing fine by its Id. The fine must not be paid or associated with a closed loan."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteFine(int id)
        {
            var result = await mediator.Send(new DeleteFineCommand(id));
            return result.ToActionResult();
        }

        [HttpGet("un-paid-fines-admin")]
        [SwaggerOperation(
     Summary = "Get all unpaid fines",
     Description = "Retrieves a list of all unpaid fines . " +
                   "A fine is considered unpaid when the PaidAmount is less than the TotalAmount."
 )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUnpaidFines(int PageNumber, int PageSize)
        {
            var result = await mediator.Send(new GetAllUnpaidFinesQuery(PageNumber, PageSize));
            return result.ToActionResult();
        }


        [HttpGet("paid-fines-admin")]
        [SwaggerOperation(
   Summary = "Get all paid fines",
   Description = "Retrieves a list of all paid fines  " +
                 "A fine is considered paid when the PaidAmount equal the TotalAmount."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetpaidFines(int PageNumber, int PageSize)
        {
            var result = await mediator.Send(new GetAllpaidFinesQuery(PageNumber, PageSize));
            return result.ToActionResult();
        }

        [HttpGet("{id}/remaining")]
        [SwaggerOperation(
    Summary = "Get remaining amount for a fine",
    Description = "Retrieve the remaining unpaid amount for a specific fine after deducting all payments."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFineRemaining(int id)
        {
            var result = await mediator.Send(new GetFineRemainingQuery(id));

            return result.ToActionResult();
        }
        [HttpGet("fine-stats")]
        [SwaggerOperation(
    Summary = "Get fines statistics",
    Description = "Retrieve statistical data about fines, including total fines, total paid, total remaining, and overdue fines."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetFineStats()
        {
            var result = await mediator.Send(new GetFineStatsQuery());

            return result.ToActionResult();
        }

        [HttpGet("by-date")]
        [SwaggerOperation(
    Summary = "Get fines by date range",
    Description = "Retrieve a paginated list of fines within a specific date range."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFinesByDate(
    [FromQuery] DateTime from,
    [FromQuery] DateTime to,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
        {
            var result = await mediator.Send(
                new GetFinesByDateQuery(from, to, pageNumber, pageSize)
            );

            return result.ToActionResult();
        }
        [HttpGet("overdue")]
        [SwaggerOperation(
           Summary = "Get overdue fines",
           Description = "Retrieve a list of all fines that are overdue and not fully paid."
       )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOverdueFines()
        {
            var result = await mediator.Send(new GetOverdueFinesQuery());

            return result.ToActionResult();
        }

        [HttpGet("report")]
        [SwaggerOperation(
    Summary = "Get fines report",
    Description = "Retrieve a summary report of fines including totals, paid amounts, and unpaid amounts."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFinesReport()
        {
            var result = await mediator.Send(new GetFinesReportQuery());

            return result.ToActionResult();
        }

        [HttpGet("run-fine-recurring")]
        public async Task<IActionResult> RunFineRecurringJob()
        {
            RecurringJob.AddOrUpdate(
             "calculate-fines-job",
             () => fineJobs.CalculateFinesForOverdueBooks(),
             "*/10 * * * *"
         );
            return Ok("Fine recurring job executed successfully.");


        }
    }
}
