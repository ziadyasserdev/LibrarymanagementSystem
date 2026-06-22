using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Locations.Dtos;
using LibrarymanagementSystem.Data.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.GetLocationById
{
    public class GetLocationByIdQueryHandler : IRequestHandler<GetLocationByIdQuery, Result<LocationDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetLocationByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<LocationDto>> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
        {
            var location = await unitOfWork.Locations.GetByIdAsync(request.LocationId);
            if (location == null )
                return Result<LocationDto>.Failure(ResultStatus.NotFound, "Location not fount");
            var locationDto = new LocationDto
            {
                Id = location.Id,
                Section = location.Section,
                Shelf = location.Shelf,
                Floor = location.Floor,
                IsActive =location.IsActive,
                BranchId= location.BranchId,
                UpdatedAt=location.UpdatedAt,
                DeletedAt = location.DeletedAt,
                Capacity=location.Capacity,
            };
            return Result<LocationDto>.Success(locationDto);

        }
    }
}
