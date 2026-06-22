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

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetFinesByDate
{
    public class GetFinesByDateQueryHandler : IRequestHandler<GetFinesByDateQuery, Result<PaginatedResult<FineByDateDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetFinesByDateQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedResult<FineByDateDto>>> Handle(GetFinesByDateQuery request, CancellationToken cancellationToken)
        {
            var baseQuery = unitOfWork.Fines.Query()
           .Where(x => x.CreatedAt >= request.From && x.CreatedAt <= request.To);

            var totalCount = await baseQuery.CountAsync(cancellationToken);

            var fines = await baseQuery
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new FineByDateDto
                {
                    Id = x.Id,
                    Amount = x.TotalAmount,
                    PaidAmount = x.PaidAmount,
                    UserId = x.LoanBook.UserId,
                    CreatedAt = x.CreatedAt,
                    BorrowRecordId = x.LoanBookId,
                    RemainingAmount = x.TotalAmount - x.PaidAmount
                })
                .ToListAsync(cancellationToken);

            return Result<PaginatedResult<FineByDateDto>>.Success(new PaginatedResult<FineByDateDto>(fines,request.PageNumber,request.PageSize,totalCount));
        }
    }
}
