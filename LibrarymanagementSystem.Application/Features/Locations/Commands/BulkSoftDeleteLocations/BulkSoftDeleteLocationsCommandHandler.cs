using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Commands.BulkSoftDeleteLocations
{
    public class BulkSoftDeleteLocationsCommandHandler : IRequestHandler<BulkSoftDeleteLocationsCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public BulkSoftDeleteLocationsCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(BulkSoftDeleteLocationsCommand request, CancellationToken cancellationToken)
        {
            var locationsId = request.LocationIds.Distinct().ToList();

            var locations = await unitOfWork.Locations.Query()
                .Where(x => locationsId.Contains(x.Id))
                .ToListAsync(cancellationToken);

            if (!locations.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No locations found for the given IDs.");

            var activeLocations = locations.Where(x => !x.IsDeleted).ToList();

            if (!activeLocations.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "All selected locations are already deleted.");

            using var transaction = await unitOfWork.BeginTransactionAsync();

            foreach (var location in activeLocations)
            {
                if (location.BookCopies.Any())
                {
                    return Result<string>.Failure(ResultStatus.Failure,
                        $"Location with ID {location.Id} cannot be deleted because it has books assigned.");
                }

                location.IsDeleted = true;
                location.DeletedAt = DateTime.UtcNow;
                location.IsActive = false;
                location.UpdatedAt = DateTime.UtcNow;
            }

            await unitOfWork.SaveAsync();
            await transaction.CommitAsync();

            return Result<string>.Success($"{activeLocations.Count} locations soft deleted successfully.");
        }
    }
}
