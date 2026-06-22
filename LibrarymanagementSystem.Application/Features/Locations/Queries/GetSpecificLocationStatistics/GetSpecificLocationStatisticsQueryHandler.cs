using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Locations.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Locations.Queries.GetSpecificLocationStatistics
{
    public class GetSpecificLocationStatisticsQueryHandler : IRequestHandler<GetSpecificLocationStatisticsQuery, Result<SpecificLocationStatisticsDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetSpecificLocationStatisticsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<SpecificLocationStatisticsDto>> Handle(GetSpecificLocationStatisticsQuery request, CancellationToken cancellationToken)
        {
            var stats = await unitOfWork.Locations.Query()
                .Where(l => l.Id == request.LocationId)
                .Select(l => new SpecificLocationStatisticsDto
                {
                    Id = l.Id,
                    Shelf = l.Shelf,
                    Floor = l.Floor,
                    Section = l.Section,
                    BranchId = l.BranchId,
                    BranchName = l.Branch.Name,
                    Capacity = l.Capacity,
                    IsActive = l.IsActive,
                    IsDeleted = l.IsDeleted,
                    TotalBooks = l.BookCopies.Count(),
                    AvailableBooks = l.BookCopies.Count(x => x.Status == BookCopyStatus.Available),
                    LostBooks = l.BookCopies.Count(b => b.IsLost),
                    OccupiedSlots = l.BookCopies.Count(),
                    BorrowedBooks = unitOfWork.LoanBooks.Query()
                                    .Count(lb => lb.BookCopy.LocationId == request.LocationId)
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (stats == null)
                return Result<SpecificLocationStatisticsDto>.Failure(ResultStatus.NotFound, "Location not found");

            return Result<SpecificLocationStatisticsDto>.Success(stats);
        }
    }
}
