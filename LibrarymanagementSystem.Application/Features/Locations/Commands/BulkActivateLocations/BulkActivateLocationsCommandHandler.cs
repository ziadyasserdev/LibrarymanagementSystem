using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Commands.BulkActivateLocations
{
    public class BulkActivateLocationsCommandHandler : IRequestHandler<BulkActivateLocationsCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public BulkActivateLocationsCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(BulkActivateLocationsCommand request, CancellationToken cancellationToken)
        {
            var locationsId = request.LocationIds.Distinct().ToList();
            var locations = await unitOfWork.Locations.Query()
                .Where(x => locationsId.Contains(x.Id)).ToListAsync();
            if(!locations.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No locations found");
            var inActiveLocations = locations.Where(x => !x.IsDeleted && !x.IsActive).ToList();
            if(!inActiveLocations.Any())
                return Result<string>.Failure(ResultStatus.Failure, "No inactive locations found to active it");
            foreach(var location in inActiveLocations)
            {
                location.IsActive = true;
                location.UpdatedAt = DateTime.Now;
            }
            await unitOfWork.SaveAsync();
            return Result<string>.Success($"{inActiveLocations.Count} Locations activated successfully");
        }
    }
}
