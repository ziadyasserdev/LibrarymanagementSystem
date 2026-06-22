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

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.GetEmptyLocations
{
    public class GetEmptyLocationsQueryHandler : IRequestHandler<GetEmptyLocationsQuery, Result<List<EmptyLocationDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetEmptyLocationsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<EmptyLocationDto>>> Handle(GetEmptyLocationsQuery request, CancellationToken cancellationToken)
        {
            var locations = await unitOfWork.Locations.Query()
                 .AsNoTracking()
                 .Where(x => !x.BookCopies.Any() && !x.IsDeleted)
                 .Select(x => new EmptyLocationDto
                 {
                     Id = x.Id,
                     Floor = x.Floor,
                     Section = x.Section,
                     Shelf = x.Shelf,
                     BranchName = x.Branch.Name,
                     Capacity = x.Capacity,

                 }).ToListAsync(cancellationToken);
            if (!locations.Any())
                return Result<List<EmptyLocationDto>>.Failure(ResultStatus.NotFound, "No empty locations found");
            return Result<List<EmptyLocationDto>>.Success(locations);
        }
    }
}
