using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using LibrarymanagementSystem.Application.Features.LoanBooks.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetAllLoanBookWithPagination
{
    public class GetAllLoanBookWithPaginationQueryHandler : IRequestHandler<GetAllLoanBookWithPaginationQuery, Result<PaginatedResult<LoanBookDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllLoanBookWithPaginationQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<PaginatedResult<LoanBookDto>>> Handle(GetAllLoanBookWithPaginationQuery request, CancellationToken cancellationToken)
        {

            var query = unitOfWork.LoanBooks
                .Query()
                .Include(x => x.BookCopy)
                .ThenInclude(x => x.Book)
                .Include(x => x.User)
                .AsNoTracking();
            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new LoanBookDto
                {
                    Id = x.Id,
                    LoanDate = x.LoanDate,
                    ReturnDate = x.ReturnDate ?? DateTime.MinValue,
                    DueDate = x.DueDate,
                    BookTitle = x.BookCopy.Book.Title,
                    UserName = x.User.FullName,
                    IsReturned = x.ReturnDate.HasValue ? true : false
                }).ToListAsync();

            var paginatedResult = new PaginatedResult<LoanBookDto>(
    items,
    request.PageNumber,
    request.PageSize,
    totalCount
);

            return Result<PaginatedResult<LoanBookDto>>.Success(paginatedResult);
        }
    }
}
