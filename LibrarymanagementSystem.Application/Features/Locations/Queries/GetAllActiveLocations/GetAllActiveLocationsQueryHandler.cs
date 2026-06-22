using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Locations.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.GetAllActiveLocations
{
    public class GetAllActiveLocationsQueryHandler : IRequestHandler<GetAllActiveLocationsQuery, Result<List<LocationStatusDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllActiveLocationsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<LocationStatusDto>>> Handle(GetAllActiveLocationsQuery request, CancellationToken cancellationToken)
        {
            var locations = await unitOfWork.Locations.Query()
                .Where(x => !x.IsDeleted && x.IsActive)
                .ToListAsync();
            if (locations == null || !locations.Any())
                return Result<List<LocationStatusDto>>.Failure(ResultStatus.NotFound, "We don't found active locations");
            var locationsDto = locations.Select(x => new LocationStatusDto
            {
                Id = x.Id,
                Floor = x.Floor,
                Section = x.Section,
                Shelf = x.Shelf,
                BranchId= x.BranchId,
                Capacity= x.Capacity,

            }).ToList();
            return Result<List<LocationStatusDto>>.Success(locationsDto);
        }
    }
}
