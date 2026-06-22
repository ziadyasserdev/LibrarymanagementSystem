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

namespace LibrarymanagementSystem.Application.Features.Reservations.Queries.GetReservationById
{
    public class GetReservationByIdQueryHandler
     : IRequestHandler<GetReservationByIdQuery, Result<AdminReservationDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetReservationByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<AdminReservationDto>> Handle(
            GetReservationByIdQuery request,
            CancellationToken cancellationToken)
        {
            var reservation = await unitOfWork.Reservations.Query()
                .Include(r => r.Book)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == request.Id);

            if (reservation == null)
                return Result<AdminReservationDto>.Failure(
                    ResultStatus.NotFound,
                    "Reservation not found");

            var dto = new AdminReservationDto
            {
                Id = reservation.Id,
                UserName = reservation.User.UserName,
                UserEmail = reservation.User.Email,
                BookTitle = reservation.Book.Title,
                QueuePosition = reservation.QueuePosition,
                Status = reservation.Status.ToString(),
                ReservationDate = reservation.ReservationDate,
                ExpirationDate = reservation.ExpirationDate
            };

            return Result<AdminReservationDto>.Success(dto);
        }
    }
}
