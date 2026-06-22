using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Locations.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.CanDeleteLocation
{
    public class CanDeleteLocationQueryHandler : IRequestHandler<CanDeleteLocationQuery, Result<CanDeleteLocationDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public CanDeleteLocationQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<CanDeleteLocationDto>> Handle(CanDeleteLocationQuery request, CancellationToken cancellationToken)
        {
            var locationData = await unitOfWork.Locations
               .Query()
                .Where(x => x.Id == request.Id)
            .Select(x => new
            {
                x.Id,
                x.IsDeleted
            })
              .FirstOrDefaultAsync(cancellationToken);

            if (locationData == null)
                return Result<CanDeleteLocationDto>.Failure(
                    ResultStatus.NotFound,
                    "Location not found");


            if (locationData.IsDeleted)
            {
                return Result<CanDeleteLocationDto>.Success(
                    new CanDeleteLocationDto
                    {
                        LocationId = request.Id,
                        CanDelete = false,
                        Reason = "Location already deleted"
                    });
            }


            var hasBookCopies = await unitOfWork.BookCopies
            .Query()
            .AnyAsync(
            x => x.LocationId == request.Id,
            cancellationToken);

            
            return Result<CanDeleteLocationDto>.Success(
            new CanDeleteLocationDto
            {
                LocationId = request.Id,
                CanDelete = !hasBookCopies,
                Reason = hasBookCopies
                ? "Location contains books"
                : null
            });
        }
    }
}
