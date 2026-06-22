using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.LoanBooks.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetLostBooksWithPagination
{
    public class GetLostBooksWithPaginationQueryHandler : IRequestHandler<GetLostBooksWithPaginationQuery, Result<PaginatedResult<LostBookDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetLostBooksWithPaginationQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<PaginatedResult<LostBookDto>>> Handle(GetLostBooksWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.LoanBooks.Query()
                .Include(x => x.BookCopy)
                .ThenInclude(x => x.Book)
                .Where(x => x.Fine != null && x.Fine.FineType == FineType.LostBook )
                .AsNoTracking();

            var items = await query.OrderBy(x => x.Fine!.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new LostBookDto
                {
                    Id = x.Id,
                    BookCopyId = x.BookCopyId,
                    BookTitle = x.BookCopy.Book.Title,
                    ISBN = x.BookCopy.Book.ISBN,
                    UserId = x.User.Id,
                    UserFullName = x.User.FullName,
                    LostDate = x.Fine != null ? x.Fine.CreatedAt : null,
                    Notes = x.Fine != null ? x.Fine.Notes : string.Empty
                }).ToListAsync();

            var totalCount = items.Count();
            var paginatedResult = new PaginatedResult<LostBookDto>(items, request.PageNumber, request.PageSize, totalCount);
            return Result<PaginatedResult<LostBookDto>>.Success(paginatedResult);
        }
    }
}
