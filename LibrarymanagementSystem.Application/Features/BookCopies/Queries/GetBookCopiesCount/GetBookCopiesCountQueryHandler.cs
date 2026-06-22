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

namespace LibrarymanagementSystem.Application.Features.BookCopies.Queries.GetBookCopiesCount
{
    public class GetBookCopiesCountQueryHandler : IRequestHandler<GetBookCopiesCountQuery, Result<BookCopiesCountDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetBookCopiesCountQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<BookCopiesCountDto>> Handle(GetBookCopiesCountQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.BookCopies
           .Query()
           .IgnoreQueryFilters();

            var counts = await query
                .GroupBy(x => 1)
                .Select(g => new BookCopiesCountDto
                {
                    Total = g.Count(),
                    Available = g.Count(x => x.Status == BookCopyStatus.Available && !x.IsDeleted),
                    Loaned = g.Count(x => x.Status == BookCopyStatus.Loaned && !x.IsDeleted),
                    Reserved = g.Count(x => x.Status == BookCopyStatus.Reserved && !x.IsDeleted),
                    Lost = g.Count(x => x.IsLost),
                    Damaged = g.Count(x => x.IsDamaged),
                    Deleted = g.Count(x => x.IsDeleted)
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (counts == null)
                counts = new BookCopiesCountDto();

            return Result<BookCopiesCountDto>.Success(counts);
        }
    }
}
