using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.FinePayments.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetFinePaymentById
{
    public class GetFinePaymentByIdQueryHandler : IRequestHandler<GetFinePaymentByIdQuery, Result<FinePaymentDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetFinePaymentByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

      
        public async Task<Result<FinePaymentDto>> Handle(GetFinePaymentByIdQuery request, CancellationToken cancellationToken)
        {
            var payment=await unitOfWork.FinePayments.GetByIdAsync(request.Id);
            if (payment == null)
              return  Result<FinePaymentDto>.Failure(ResultStatus.NotFound,"FinePayment not found");
            var paymentDto = new FinePaymentDto
            {
                Id= payment.Id,
                FineId= payment.Id,
                Amount= payment.Amount,
                PaymentDate= payment.PaymentDate,
                PaymentMethod= payment.PaymentMethod,   
                Notes= payment.Notes,
            };

            return Result<FinePaymentDto>.Success(paymentDto);
        }
    }
}
