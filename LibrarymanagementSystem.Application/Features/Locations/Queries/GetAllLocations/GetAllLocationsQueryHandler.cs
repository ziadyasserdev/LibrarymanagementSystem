using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Locations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.GetAllLocations
{
    public class GetAllLocationsQueryHandler : IRequestHandler<GetAllLocationsQuery, Result<List<LocationDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllLocationsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<LocationDto>>> Handle(GetAllLocationsQuery request, CancellationToken cancellationToken)
        {
            var locations = await unitOfWork.Locations.GetAllAsync();
            if (locations == null||!locations.Any())
                return Result<List<LocationDto>>.Failure(ResultStatus.NotFound, "Locations not fount");
            var locationsDto=locations.Select(x=> new LocationDto
            {
                Id=x.Id,
                Floor=x.Floor,
                Section=x.Section,
                Shelf=x.Shelf,
                Capacity=x.Capacity,
                BranchId=x.BranchId,
                IsActive =x.IsActive,
                DeletedAt=x.DeletedAt,
               UpdatedAt=x.UpdatedAt,
            }).ToList();
            return Result<List<LocationDto>>.Success(locationsDto);
        }
    }
}
