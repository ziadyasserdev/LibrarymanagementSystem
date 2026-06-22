using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Locations.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.SearchLocations
{
    public class SearchLocationsQueryHandler :
        IRequestHandler<SearchLocationsQuery, Result<List<LocationSearchDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public SearchLocationsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<List<LocationSearchDto>>> Handle(
            SearchLocationsQuery request,
            CancellationToken cancellationToken)
        {

            var query = unitOfWork.Locations
                .Query()
                .Where(x => !x.IsDeleted)
                .Include(x => x.Branch)
                .Include(x => x.BookCopies)
                .ThenInclude(x => x.Book)
                .AsQueryable();


            if (!string.IsNullOrEmpty(request.Shelf))
            {
                query = query.Where(x =>
                    x.Shelf.Contains(request.Shelf));
            }


            if (!string.IsNullOrEmpty(request.Floor))
            {
                query = query.Where(x =>
                    x.Floor.Contains(request.Floor));
            }


            if (!string.IsNullOrEmpty(request.Section))
            {
                query = query.Where(x =>
                    x.Section.Contains(request.Section));
            }


            if (request.BranchId.HasValue)
            {
                query = query.Where(x =>
                    x.BranchId == request.BranchId);
            }


            var result = await query
                .Select(x => new LocationSearchDto
                {
                    Id = x.Id,

                    Shelf = x.Shelf,

                    Floor = x.Floor,

                    Section = x.Section,

                    BranchName = x.Branch.Name,

                    BooksCount = x.BookCopies.Count
                })
                .ToListAsync();


            return Result<List<LocationSearchDto>>
                .Success(result);
        }
    }
}