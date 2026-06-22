using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Reservations.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reservations.Queries.GetAllReservations
{
    public class GetAllReservationsQueryHandler
     : IRequestHandler<GetAllReservationsQuery, Result<List<AdminReservationDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllReservationsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<List<AdminReservationDto>>> Handle(
            GetAllReservationsQuery request,
            CancellationToken cancellationToken)
        {
            var reservations = await unitOfWork.Reservations.Query()
                .Include(r => r.Book)
                .Include(r => r.User)
                .OrderByDescending(r => r.ReservationDate)
                .Select(r => new AdminReservationDto
                {
                    Id = r.Id,
                    UserName = r.User.UserName,
                    UserEmail = r.User.Email,
                    BookTitle = r.Book.Title,
                    QueuePosition = r.QueuePosition,
                    Status = r.Status.ToString(),
                    ReservationDate = r.ReservationDate,
                    ExpirationDate = r.ExpirationDate
                })
                .ToListAsync();

            return Result<List<AdminReservationDto>>.Success(reservations);
        }
    }
}
