using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Fines.Dtos;
using LibrarymanagementSystem.Application.Features.Fines.Queries.GetAllUnpaidFines;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetAllPaidFines
{
    public class GetAllpaidFinesQueryHandler : IRequestHandler<GetAllpaidFinesQuery, Result<PaginatedResult<FineDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllpaidFinesQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public async Task<Result<PaginatedResult<FineDto>>> Handle(GetAllpaidFinesQuery request, CancellationToken cancellationToken)
        {
            var baseQuery = unitOfWork.Fines.Query()
              .Where(x => x.PaidAmount == x.TotalAmount);

            var totalCount = await baseQuery.CountAsync();

            var paidFines = await baseQuery
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new FineDto
                {
                    Id = x.Id,
                    LoanBookId = x.LoanBookId,
                    BookTitle = x.LoanBook.BookCopy.Book.Title,
                    UserId = x.LoanBook.User.Id,
                    UserName = x.LoanBook.User.FullName,
                    PaidAmount = x.PaidAmount,
                    TotalAmount = x.TotalAmount,
                    FineType = x.FineType,
                    LoanStatus = x.LoanBook.Status,
                    CreatedAt = x.CreatedAt,
                })
                .ToListAsync();

            var paginatedResult = new PaginatedResult<FineDto>(
                paidFines,
                request.PageNumber,
                request.PageSize,
                totalCount
            );
            return Result<PaginatedResult<FineDto>>.Success(paginatedResult);
        }
    }
}
