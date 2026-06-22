using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.BookCopies.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.BookCopies.Queries.GetBookCopiesStatistics
{
    public class GetBookCopiesStatisticsQueryHandler : IRequestHandler<GetBookCopiesStatisticsQuery, Result<BookCopyStatisticsDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetBookCopiesStatisticsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<BookCopyStatisticsDto>> Handle(GetBookCopiesStatisticsQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.BookCopies.Query();
                
            if(request.BookId.HasValue)
                query=query.Where(x => x.BookId == request.BookId.Value);
            if (request.BranchId.HasValue)
                query = query.Where(x => x.Location.BranchId == request.BranchId.Value);
            if (request.LocationId.HasValue)
                query = query.Where(x => x.LocationId == request.LocationId.Value);
            var stats = query.GroupBy(x => 1)
.Select(c => new BookCopyStatisticsDto
{
    TotalCopies = c.Count(),
    Available=c.Count(x => x.Status == BookCopyStatus.Available && !x.IsDeleted),
    Damaged=c.Count(x => x.Status == BookCopyStatus.Damaged && !x.IsDeleted),
    Deleted=c.Count(x => x.IsDeleted),
    Loaned=c.Count(x => x.Status == BookCopyStatus.Loaned && !x.IsDeleted),
    Lost=c.Count(x => x.Status == BookCopyStatus.Lost && !x.IsDeleted),
    Reserved=c.Count(x => x.Status == BookCopyStatus.Reserved && !x.IsDeleted)
});
            var result = await stats.FirstOrDefaultAsync(cancellationToken);
            if(result == null)
            {
                result = new BookCopyStatisticsDto();
            }
            return Result<BookCopyStatisticsDto>.Success(result);
        }
    }
}
