using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Reservations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reservations.Commands.CreateReservation
{
    public class CreateReservationCommand : IRequest<Result<ReservationDto>>
    {
     
        public int BookId { get; set; }
    }
}
