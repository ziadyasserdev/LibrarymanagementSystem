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

namespace LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetAllFinePaymentsOfUser
{
    public class GetAllFinePaymentsOfUserQueryHandler : IRequestHandler<GetAllFinePaymentsOfUserQuery, Result<List<FinePaymentDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllFinePaymentsOfUserQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
       
        public async Task<Result<List<FinePaymentDto>>> Handle(GetAllFinePaymentsOfUserQuery request, CancellationToken cancellationToken)
        {
            var payments = await unitOfWork.FinePayments.Query()
                 .Where(x => x.Fine.LoanBook.UserId == request.userId)
                 .ToListAsync();
            var paymentsDto = payments.Select(x => new FinePaymentDto
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
