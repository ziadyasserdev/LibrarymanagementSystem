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

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.GetLocationsByBranch
{
    public class GetLocationsByBranchQueryHandler : IRequestHandler<GetLocationsByBranchQuery, Result<List<LocationListDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetLocationsByBranchQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<LocationListDto>>> Handle(GetLocationsByBranchQuery request, CancellationToken cancellationToken)
        {
            var branch = await unitOfWork.Branches.Query()
                 .FirstOrDefaultAsync(x => x.Id == request.BranchId && !x.IsDeleted);
            if (branch == null)
                return Result<List<LocationListDto>>.Failure(ResultStatus.NotFound, "Branch not found");
            var locations = await unitOfWork.Locations.Query()
                .AsNoTracking()
                .Where(x => x.BranchId == request.BranchId && !x.IsDeleted)
                .Select(x => new LocationListDto
                {
                    Id = x.Id,
                    Floor = x.Floor,
                    Section = x.Section,
                    Shelf = x.Shelf,
                    BooksCount = x.BookCopies.Count,
                    IsActive = x.IsActive,
                    Capacity = x.Capacity,
                }).ToListAsync(cancellationToken);
            if (!locations.Any())
                return Result<List<LocationListDto>>.Failure(ResultStatus.NotFound, "No locations found for this branch");
            return Result<List<LocationListDto>>.Success(locations);
        }
    }
}
