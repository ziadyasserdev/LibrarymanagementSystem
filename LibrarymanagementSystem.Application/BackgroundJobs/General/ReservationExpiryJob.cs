using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Reservations.Commands.PromoteReservation;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.BackgroundJobs.General
{
    public class ReservationExpiryJob
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;

        public ReservationExpiryJob(IUnitOfWork unitOfWork, IMediator mediator)
        {
            this.unitOfWork = unitOfWork;
            this.mediator = mediator;
        }

        public async Task Execute()
        {
            var now = DateTime.UtcNow;

          
            var expiredReservations = await unitOfWork.Reservations.Query()
                .Where(r =>
                    r.Status == ReservationStatus.ReadyForPickup &&
                    r.ExpirationDate <= now)
                .ToListAsync();

            if (!expiredReservations.Any())
                return;

            foreach (var reservation in expiredReservations)
            {
                
                reservation.Status = ReservationStatus.Expired;

             
               
                await unitOfWork.SaveAsync();

             
                await mediator.Send(new PromoteReservationCommand
                {
                    BookId = reservation.BookId
                });
            }
        }
    }
}
