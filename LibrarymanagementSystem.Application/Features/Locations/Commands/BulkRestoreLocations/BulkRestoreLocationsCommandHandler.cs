using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Commands.BulkRestoreLocations
{
    public class BulkRestoreLocationsCommandHandler : IRequestHandler<BulkRestoreLocationsCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public BulkRestoreLocationsCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(BulkRestoreLocationsCommand request, CancellationToken cancellationToken)
       {
            var locationsId = request.LocationIds.Distinct().ToList();

           
            var locations = await unitOfWork.Locations.Query()
                .Where(x => locationsId.Contains(x.Id))
                .ToListAsync(cancellationToken);

            if (!locations.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No locations found");

           
            var deletedLocations = locations.Where(x => x.IsDeleted).ToList();
            if (!deletedLocations.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No deleted locations found to restore");

         
            using var transaction = await unitOfWork.BeginTransactionAsync();

            try
            {
                foreach (var location in deletedLocations)
                {
                    location.IsDeleted = false;
                    location.DeletedAt = null;
                    location.UpdatedAt = DateTime.UtcNow;
                }

                await unitOfWork.SaveAsync();

                await transaction.CommitAsync(cancellationToken);

                return Result<string>.Success($"{deletedLocations.Count} Locations restored successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
               
                return Result<string>.Failure(ResultStatus.Failure, $"Error restoring locations: {ex.Message}");
            }
        }

    }
            
    }

