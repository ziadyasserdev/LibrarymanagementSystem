using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.LoanBooks.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetLoanBooksStatistics
{
    public class GetLoanBooksStatisticsQueryHandler : IRequestHandler<GetLoanBooksStatisticsQuery, Result<LibraryDashboardDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetLoanBooksStatisticsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
   
        public async Task<Result<LibraryDashboardDto>> Handle(GetLoanBooksStatisticsQuery request, CancellationToken cancellationToken)
        {
            var dashboard = new LibraryDashboardDto
            {
                ActiveLoans = await unitOfWork.LoanBooks.Query().CountAsync(lb => lb.ReturnDate == null && lb.Status == Data.Enum.LoanStatus.Active),
                LateLoans = await unitOfWork.LoanBooks.Query()
         .CountAsync(lb => lb.ReturnDate == null && lb.DueDate < DateTime.Now &&
                    lb.Status != LoanStatus.Lost),
                LostBooks = await unitOfWork.LoanBooks.Query().CountAsync(b => b.Status == Data.Enum.LoanStatus.Lost),
                TotalFinesCollected = await unitOfWork.Fines.Query()
         .Where(f => f.PaidAmount == f.TotalAmount)
         .SumAsync(f => f.TotalAmount),
                MostBorrowedBook = await unitOfWork.LoanBooks.Query()
         .GroupBy(lb => lb.BookCopyId)
         .OrderByDescending(g => g.Count())
         .Select(g => g.FirstOrDefault()!.BookCopy.Book.Title)
         .FirstOrDefaultAsync() ?? "No records"
            };

            return Result<LibraryDashboardDto>.Success(dashboard);
        }
    }
}
