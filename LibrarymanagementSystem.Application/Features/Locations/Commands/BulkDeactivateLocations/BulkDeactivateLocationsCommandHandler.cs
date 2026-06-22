using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Commands.BulkDeactivateLocations
{
    public class BulkDeactivateLocationsCommandHandler : IRequestHandler<BulkDeactivateLocationsCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public BulkDeactivateLocationsCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(BulkDeactivateLocationsCommand request, CancellationToken cancellationToken)
        {
            var locationsId = request.LocationIds.Distinct().ToList();
            var locations = await unitOfWork.Locations.Query()
                .Where(x => locationsId.Contains(x.Id)).ToListAsync();
            if (!locations.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No locations found");
            var activeLocations = locations
                .Where(x => !x.IsDeleted && x.IsActive).ToList();

            if (!activeLocations.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No active locations found to inactive it");

            foreach (var location in activeLocations)
            {
                var hasBooks = await unitOfWork.BookCopies.Query()
      .AnyAsync(b => b.LocationId == location.Id);

                if (hasBooks)
                    return Result<string>.Failure(ResultStatus.Failure, $"Location with id {location.Id} cannot be deactivated because it has books assigned to it");

                location.IsActive = false;
                location.UpdatedAt = DateTime.Now;
            }
            await unitOfWork.SaveAsync();
            return Result<string>.Success($"{activeLocations.Count} Locations deactivated successfully");
        }
    }
}
