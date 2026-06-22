using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reservations.Commands.CancelReservation
{
    public class CancelReservationCommand : IRequest<Result<bool>>
    {
        public int ReservationId { get; set; }
    }
}
