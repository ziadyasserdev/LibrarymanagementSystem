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

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.GetLocationCapacity
{
    public class GetLocationCapacityQueryHandler : IRequestHandler<GetLocationCapacityQuery, Result<LocationCapacityDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetLocationCapacityQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<LocationCapacityDto>> Handle(GetLocationCapacityQuery request, CancellationToken cancellationToken)
        {
            var location = await unitOfWork.Locations.Query()
                .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted && x.IsActive);
            if (location == null)
                return Result<LocationCapacityDto>.Failure(ResultStatus.NotFound, "Location not found");
            var locationCapacityDto = new LocationCapacityDto
            {
                LocationId = location.Id,
                TotalCapacity = location.Capacity,
                UsedCapacity = unitOfWork.BookCopies.Query()
                .Where(x => x.LocationId == request.Id)
                .Count(),

            };
            var availableCapacity = location.Capacity - locationCapacityDto.UsedCapacity;
            locationCapacityDto.AvailableCapacity = availableCapacity;
            return Result<LocationCapacityDto>.Success(locationCapacityDto);
        }
    }
}
