using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Reservations.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reservations.Queries.GetMyReservations
{
    public class GetMyReservationsQueryHandler
     : IRequestHandler<GetMyReservationsQuery, Result<List<MyReservationDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetMyReservationsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<MyReservationDto>>> Handle(
            GetMyReservationsQuery request,
            CancellationToken cancellationToken)
        {
            // 1. Auth check
            if (!currentUserService.IsAuthenticated)
                return Result<List<MyReservationDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "User must be logged in");

            var userId = currentUserService.UserId;

           
            var reservations = await unitOfWork.Reservations.Query()
                .Include(r => r.Book)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.ReservationDate)
                .Select(r => new MyReservationDto
                {
                    Id = r.Id,
                    BookTitle = r.Book.Title,
                    QueuePosition = r.QueuePosition,
                    Status = r.Status.ToString(),
                    ReservationDate = r.ReservationDate,
                    ExpirationDate = r.ExpirationDate
                })
                .ToListAsync();

            return Result<List<MyReservationDto>>.Success(reservations);
        }
    }
}
