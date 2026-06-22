using LibrarymanagementSystem.Application.Common.PaginatedResults;
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

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.GetAllLocationsWithPagination
{
    public class GetAllLocationsWithPaginationQueryHandler : IRequestHandler<GetAllLocationsWithPaginationQuery, Result<PaginatedResult<LocationDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllLocationsWithPaginationQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<PaginatedResult<LocationDto>>> Handle(GetAllLocationsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Locations.Query();
            var totalCount = await query.CountAsync();
            var items =await query.OrderBy(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(v => new LocationDto
                {
                    Id=v.Id,
                    Floor=v.Floor,
                    Section=v.Section,
                    Shelf=v.Shelf,
                    Capacity=v.Capacity,
                    IsActive =v.IsActive,
                    BranchId=v.BranchId,
                    UpdatedAt=v.UpdatedAt,
                    DeletedAt =v.DeletedAt
                }).ToListAsync();
            var paginatedResult = new PaginatedResult<LocationDto>(items, request.PageNumber, request.PageSize, totalCount);
            return Result<PaginatedResult<LocationDto>>.Success(paginatedResult);
        }
    }
}
