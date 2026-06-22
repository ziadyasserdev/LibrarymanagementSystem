using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reservations.Commands.CancelReservation
{
    public class CancelReservationCommandHandler
    : IRequestHandler<CancelReservationCommand, Result<bool>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public CancelReservationCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(
            CancelReservationCommand request,
            CancellationToken cancellationToken)
        {
            
            if (!currentUserService.IsAuthenticated)
                return Result<bool>.Failure(
                    ResultStatus.Unauthorized,
                    "User must be logged in to cancel reservation");

           
            var reservation = await unitOfWork.Reservations.Query()
                .FirstOrDefaultAsync(r =>
                    r.Id == request.ReservationId &&
                    r.UserId == currentUserService.UserId);

            if (reservation == null)
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Reservation not found");

            if (reservation.Status == ReservationStatus.Completed)
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Cannot cancel completed reservation");

           
            reservation.Status = ReservationStatus.Cancelled;
            reservation.CancelledAt = DateTime.UtcNow;

           
            var affectedReservations = await unitOfWork.Reservations.Query()
                .Where(r =>
                    r.BookId == reservation.BookId &&
                    r.Status == ReservationStatus.Pending &&
                    r.QueuePosition > reservation.QueuePosition)
                .ToListAsync();

            foreach (var r in affectedReservations)
            {
                r.QueuePosition -= 1;
            }

            await unitOfWork.SaveAsync();

            return Result<bool>.Success(true);
        }
    }
}
