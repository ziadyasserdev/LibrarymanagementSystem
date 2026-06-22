using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Fines.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetFinesReport
{
    /*
            public decimal TotalOverdueFines { get; set; }      
        public decimal TotalPaidFines { get; set; }         
        public double AverageDelayMinutes { get; set; }    
        public int LateFinesCount { get; set; }             
        public int LostFinesCount { get; set; }     
     */
    public class GetFinesReportQueryHandler : IRequestHandler<GetFinesReportQuery, Result<FineReportDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetFinesReportQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<FineReportDto>> Handle(GetFinesReportQuery request, CancellationToken cancellationToken)
        {
            var fines = await unitOfWork.Fines.Query()
           .Include(f => f.LoanBook)
               .ThenInclude(lb => lb.BookCopy)
           .ToListAsync(cancellationToken);

            if (!fines.Any())
                return Result<FineReportDto>.Success(new FineReportDto());
            var TotalOverdueFines = fines
                .Where(x => x.PaidAmount < x.TotalAmount 
                && x.CreatedAt < DateTime.UtcNow)
                .Sum(x => x.TotalAmount);
           var TotalPaidFines = fines
                .Where(x => x.PaidAmount >= x.TotalAmount)
                .Sum(x => x.PaidAmount);
          
            var lateCount = fines.Count(f => f.FineType == FineType.LateReturn);
            var lostCount = fines.Count(f => f.FineType == FineType.LostBook);
          
            var report = new FineReportDto
            {
                TotalOverdueFines = TotalOverdueFines,
                TotalPaidFines = TotalPaidFines,
              
                LateFinesCount = lateCount,
                LostFinesCount = lostCount
            };

            return Result<FineReportDto>.Success(report);

        }
    }
}
