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

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.GetLocationStatistics
{
    public class GetLocationStatisticsQueryHandler : IRequestHandler<GetLocationStatisticsQuery, Result<LocationStatisticsDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetLocationStatisticsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<LocationStatisticsDto>> Handle(GetLocationStatisticsQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Locations
                .Query();

            var statistics = await query
       .GroupBy(x => 1)
       .Select(g => new
       {
           TotalLocations = g.Count(x => !x.IsDeleted),
           ActiveLocations = g.Count(x => x.IsActive && !x.IsDeleted),
           DeletedLocations = g.Count(x => x.IsDeleted),


       })
       .FirstOrDefaultAsync(cancellationToken);

            var emptyLocations = await query
                .Where(x => !x.IsDeleted)
                .Where(x => !x.BookCopies.Any())
                .CountAsync(cancellationToken);
            var totalBooks = await unitOfWork.Books
                .Query()
                .CountAsync(cancellationToken);

            return Result<LocationStatisticsDto>.Success(
       new LocationStatisticsDto
       {
           TotalLocations = statistics!.TotalLocations,
           ActiveLocations = statistics.ActiveLocations,
           DeletedLocations = statistics.DeletedLocations,
           EmptyLocations = emptyLocations,
           TotalBooks = totalBooks,

       });
        }
    }
}
