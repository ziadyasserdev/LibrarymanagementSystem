using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.ExternalServices;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Reservations.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reservations.Commands.PromoteReservation
{
    public class PromoteReservationCommandHandler
       : IRequestHandler<PromoteReservationCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailService emailService;

        public PromoteReservationCommandHandler(
            IUnitOfWork unitOfWork,
            IEmailService emailService)
        {
            this.unitOfWork = unitOfWork;
            this.emailService = emailService;
        }

        public async Task<Result<string>> Handle(
            PromoteReservationCommand request,
            CancellationToken cancellationToken)
        {
            var book = await unitOfWork.Books.Query()
              .Include(b => b.Copies)
              .FirstOrDefaultAsync(b => b.Id == request.BookId);

            if (book == null)
                return Result<string>.Failure(ResultStatus.NotFound, "Book not found");

            var availableCopy = await unitOfWork.BookCopies.Query()
      .FirstOrDefaultAsync(c =>
          c.BookId == request.BookId &&
          c.Status == BookCopyStatus.Available &&
          !c.IsDeleted);

            if (availableCopy == null)
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "No available book copies to assign reservation");



            var reservation = await unitOfWork.Reservations.Query()
                .Include(c => c.User)
         .Where(r =>
             r.BookId == request.BookId &&
             r.Status == ReservationStatus.Pending)
         .OrderBy(r => r.QueuePosition)
         .FirstOrDefaultAsync();

            if (reservation == null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "No pending reservations found");

          
            reservation.Status = ReservationStatus.ReadyForPickup;
        
            reservation.ExpirationDate = DateTime.Now.AddHours(24);

           

            await unitOfWork.SaveAsync();

        


            await emailService.SendEmailAsync(
                reservation.User.Email,
                " Your Book is Ready",
                $@"
                Hello {reservation.User.UserName},

                Your reserved book '{reservation.Book.Title}' is now available.

                 Please pick it up within 24 hours.
                 Available until: {reservation.ExpirationDate}

                Thank you.
            ");

            return Result<string>.Success("Reservation has been successfully promoted");
        }
    }
}
