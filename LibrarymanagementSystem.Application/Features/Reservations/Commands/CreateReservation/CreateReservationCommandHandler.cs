using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Reservations.Dtos;
using LibrarymanagementSystem.Data.Enum;
using LibrarymanagementSystem.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reservations.Commands.CreateReservation
{
    public class CreateReservationCommandHandler
    : IRequestHandler<CreateReservationCommand, Result<ReservationDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public CreateReservationCommandHandler(IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<ReservationDto>> Handle(
            CreateReservationCommand request,
            CancellationToken cancellationToken)
        {
           if(!currentUserService.IsAuthenticated)
                return Result<ReservationDto>.Failure(ResultStatus.Unauthorized, "User is not authenticated");
            var userId = currentUserService.UserId;
            var book = await unitOfWork.Books.Query()
                .Include(b => b.Copies)
                .FirstOrDefaultAsync(b => b.Id == request.BookId);

            if (book == null)
                return Result<ReservationDto>.Failure(ResultStatus.NotFound, "Book not found");

         
            var availableCopies = book.Copies
                .Count(c => c.Status == BookCopyStatus.Available && !c.IsDeleted);

            if (availableCopies > 0)
                return Result<ReservationDto>.Failure(
                    ResultStatus.Failure,
                    "Book is available. You can borrow it instead of reserving.");

           
            var exists = await unitOfWork.Reservations.Query()
                .AnyAsync(r =>
                    r.BookId == request.BookId &&
                    r.UserId == userId &&
                    (r.Status == ReservationStatus.Pending ||
                     r.Status == ReservationStatus.ReadyForPickup));

            if (exists)
                return Result<ReservationDto>.Failure(
                    ResultStatus.Failure,
                    "You already have an active reservation for this book");

          
            var lastPosition = await unitOfWork.Reservations.Query()
                .Where(r => r.BookId == request.BookId)
                .OrderByDescending(r => r.QueuePosition)
                .Select(r => (int?)r.QueuePosition)
                .FirstOrDefaultAsync();

            int newPosition = (lastPosition ?? 0) + 1;

          
            var reservation = new Reservation
            {
                UserId = userId,
                BookId = request.BookId,
                ReservationDate = DateTime.Now,
                Status = ReservationStatus.Pending,
                QueuePosition = newPosition
            };

            await unitOfWork.Reservations.AddAsync(reservation);
            await unitOfWork.SaveAsync();

          
            var result = new ReservationDto
            {
                Id = reservation.Id,
                BookTitle = book.Title,
                QueuePosition = reservation.QueuePosition,
                Status = reservation.Status.ToString(),
                ReservationDate = reservation.ReservationDate
            };

            return Result<ReservationDto>.Success(result);
        }
    }
}
