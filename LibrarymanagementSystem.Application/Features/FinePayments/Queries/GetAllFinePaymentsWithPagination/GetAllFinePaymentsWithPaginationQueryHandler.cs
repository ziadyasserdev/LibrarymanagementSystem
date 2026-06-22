using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.FinePayments.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetAllFinePaymentsWithPagination
{
    public class GetAllFinePaymentsWithPaginationQueryHandler : IRequestHandler<GetAllFinePaymentsWithPaginationQuery, Result<PaginatedResult<FinePaymentDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllFinePaymentsWithPaginationQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
      
        public async Task<Result<PaginatedResult<FinePaymentDto>>> Handle(GetAllFinePaymentsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.FinePayments.Query();
            var totalCount = await query.CountAsync();
            var items=await query.OrderByDescending(x => x.PaymentDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new FinePaymentDto
                {
                    Id=x.Id,
                    FineId=x.FineId,
                    Amount=x.Amount,
                    PaymentDate=x.PaymentDate,
                    PaymentMethod=x.PaymentMethod,
                    Notes=x.Notes,
                }).ToListAsync();

            var paginatedResult = new PaginatedResult<FinePaymentDto>(items, request.PageNumber, request.PageSize, totalCount);
            return Result<PaginatedResult<FinePaymentDto>>.Success(paginatedResult);
        }
    }
}
