using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.LoanBooks.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetDamagedBooksWithPagination
{
    public class GetDamagedBooksWithPaginationQueryHandler : IRequestHandler<GetDamagedBooksWithPaginationQuery, Result<PaginatedResult<DamageBookDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetDamagedBooksWithPaginationQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<PaginatedResult<DamageBookDto>>> Handle(GetDamagedBooksWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var baseQuery = unitOfWork.LoanBooks.Query()
              .AsNoTracking()
              .Include(x => x.BookCopy)
              .ThenInclude(x => x.Book)
              .Where(x => x.Fine != null
               && x.Fine.FineType == Data.Enum.FineType.DamagedBook);

            var totalCount = await baseQuery.CountAsync(cancellationToken);

            var items = await baseQuery
                .OrderByDescending(x => x.ReturnDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new DamageBookDto
                {
                    Id = x.Id,
                    BookCopyId = x.BookCopyId,
                    BookTitle = x.BookCopy.Book.Title,
                    ISBN = x.BookCopy.Book.ISBN,
                    UserId = x.UserId,
                    UserFullName = x.User.FullName,
                    DamageDate = x.ReturnDate,
                    Notes = x.Fine!.Notes
                })
                .ToListAsync(cancellationToken);

            var paginatedResult = new PaginatedResult<DamageBookDto>(
                items,
                request.PageNumber,
                request.PageSize,
                totalCount
            );

            return Result<PaginatedResult<DamageBookDto>>.Success(paginatedResult);
        }
    }
}
