using LibrarymanagementSystem.Api.Common.Responses;
using LibrarymanagementSystem.Application.Features.Reservations.Commands.CancelReservation;
using LibrarymanagementSystem.Application.Features.Reservations.Commands.CreateReservation;
using LibrarymanagementSystem.Application.Features.Reservations.Commands.PromoteReservation;
using LibrarymanagementSystem.Application.Features.Reservations.Queries.GetAllReservations;
using LibrarymanagementSystem.Application.Features.Reservations.Queries.GetMyReservations;
using LibrarymanagementSystem.Application.Features.Reservations.Queries.GetReservationById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LibrarymanagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ReservationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        [SwaggerOperation(
          Summary = "Create reservation",
          Description = "Creates a reservation for a book by BookId."
      )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create(CreateReservationCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPost("promote")]
        [SwaggerOperation(
        Summary = "Promote reservation",
        Description = "Promotes a reservation for a specific book (e.g., moves next user in queue)."
    )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Promote([FromBody] PromoteReservationCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpDelete("{reservationId}")]
        [SwaggerOperation(
           Summary = "Cancel reservation",
           Description = "Cancels an existing reservation by its ID."
       )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Cancel(int reservationId)
        {
            var command = new CancelReservationCommand
            {
                ReservationId = reservationId
            };

            var result = await mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpGet("my")]
        [SwaggerOperation(
         Summary = "Get my reservations",
         Description = "Retrieves all reservations for the currently authenticated user."
     )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyReservations()
        {
            var result = await mediator.Send(new GetMyReservationsQuery());
            return result.ToActionResult();
        }
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        [SwaggerOperation(
          Summary = "Get all reservations",
          Description = "Returns all reservations in the system (Admin only)."
      )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAll()
        {
            var result = await mediator.Send(new GetAllReservationsQuery());
            return result.ToActionResult();
        }
        [HttpGet("{id}")]
       // [Authorize(Roles = "Admin")]
        [SwaggerOperation(
         Summary = "Get reservation by ID",
         Description = "Retrieves a specific reservation by its unique ID (Admin only)."
     )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await mediator.Send(new GetReservationByIdQuery
            {
                Id = id
            });

            return result.ToActionResult();
        }
    }
}
