using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Locations.Dtos;
using LibrarymanagementSystem.Application.Features.Locations.Queries.GetAllActiveLocations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.GetAllInActiveLocations
{
    public class GetAllInActiveLocationsQueryHandler : IRequestHandler<GetAllInActiveLocationsQuery, Result<List<LocationStatusDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllInActiveLocationsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<LocationStatusDto>>> Handle(GetAllInActiveLocationsQuery request, CancellationToken cancellationToken)
        {
            var locations = await unitOfWork.Locations.Query()
        .Where(x => !x.IsDeleted && !x.IsActive)
        .ToListAsync(cancellationToken);

            if (!locations.Any())
                return Result<List<LocationStatusDto>>.Failure(ResultStatus.NotFound, "No inactive locations found");

            var locationsDto = locations.Select(x => new LocationStatusDto
            {
                Id = x.Id,
                Floor = x.Floor,
                Section = x.Section,
                Shelf = x.Shelf,
                BranchId = x.BranchId,
                Capacity= x.Capacity,
            }).ToList();

            return Result<List<LocationStatusDto>>.Success(locationsDto);
        }
    }
}
