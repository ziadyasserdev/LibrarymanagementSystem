using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Fines.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetAllFinesWithPagination
{
    public class GetAllFinesWithPaginationQueryHandler : IRequestHandler<GetAllFinesWithPaginationQuery, Result<PaginatedResult<FineDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllFinesWithPaginationQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedResult<FineDto>>> Handle(GetAllFinesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Fines.Query()
                .Include(x => x.LoanBook);
            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(f => new FineDto
                {
                    Id = f.Id,
                    LoanBookId = f.LoanBookId,
                    BookTitle = f.LoanBook.BookCopy.Book.Title,
                    UserId = f.LoanBook.UserId,
                    UserName = f.LoanBook.User.UserName!,
                    TotalAmount = f.TotalAmount,
                    PaidAmount = f.PaidAmount,
                    FineType = f.FineType,
                    CreatedAt = f.CreatedAt,
                    LoanStatus = f.LoanBook.Status
                }).ToListAsync();

            if (!items.Any())
                Result<PaginatedResult<FineDto>>.Failure(ResultStatus.NotFound, "No Fines found");
            var paginatedResult = new PaginatedResult<FineDto>(items, request.PageNumber, request.PageSize, totalCount);
            return Result<PaginatedResult<FineDto>>.Success(paginatedResult);
        }
    }
}
