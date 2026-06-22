using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.FinePayments.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetAllFinePayments
{
    public class GetAllFinePaymentsQueryHandler : IRequestHandler<GetAllFinePaymentsQuery, Result<List<FinePaymentDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllFinePaymentsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<FinePaymentDto>>> Handle(GetAllFinePaymentsQuery request, CancellationToken cancellationToken)
        {
            var payments = await unitOfWork.FinePayments.GetAllAsync();
            var paymentsDto=payments.Select(x => new FinePaymentDto
            {
                Id=x.Id,
                FineId=x.FineId,
                Amount=x.Amount,
                PaymentDate=x.PaymentDate,
                PaymentMethod=x.PaymentMethod,
                Notes=x.Notes,
            }).ToList();

            return Result<List<FinePaymentDto>>.Success(paymentsDto);
        }
    }
}
