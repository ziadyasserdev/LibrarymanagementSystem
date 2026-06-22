using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Reservations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reservations.Queries.GetAllReservations
{
    public class GetAllReservationsQuery : IRequest<Result<List<AdminReservationDto>>>
    {
    }
}
