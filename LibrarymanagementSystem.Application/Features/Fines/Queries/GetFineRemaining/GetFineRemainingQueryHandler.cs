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

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetFineRemaining
{
    public class GetFineRemainingQueryHandler : IRequestHandler<GetFineRemainingQuery, Result<FineRemainingDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetFineRemainingQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<FineRemainingDto>> Handle(GetFineRemainingQuery request, CancellationToken cancellationToken)
        {
            var fine = await unitOfWork.Fines.Query()
        .FirstOrDefaultAsync(x => x.Id == request.FineId);

            if (fine == null)
                return Result<FineRemainingDto>.Failure(ResultStatus.NotFound, "Fine not found");

            var dto = new FineRemainingDto
            {
                FineId = fine.Id,
                TotalAmount = fine.TotalAmount,
                PaidAmount = fine.PaidAmount,
                RemainingAmount = fine.TotalAmount - fine.PaidAmount,
                IsFullyPaid = fine.PaidAmount >= fine.TotalAmount
            };

            return Result<FineRemainingDto>.Success(dto);
        }
    }
}
