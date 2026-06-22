using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Branches.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Queries.GetBranchStatistics
{
    public class GetBranchStatisticsQueryHandler : IRequestHandler<GetBranchStatisticsQuery, Result<BranchStatisticsDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetBranchStatisticsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<BranchStatisticsDto>> Handle(GetBranchStatisticsQuery request, CancellationToken cancellationToken)
        {
            var branch = await unitOfWork.Branches.Query()
                .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted);
            if (branch == null)
                return Result<BranchStatisticsDto>.Failure(ResultStatus.NotFound, "Branch not found");
            var bookCopiesQuery = unitOfWork.BookCopies.Query()
           .Where(b => b.Location.BranchId == request.Id);

            var totalBooks = await bookCopiesQuery.CountAsync(cancellationToken);




            var availableBooks = await bookCopiesQuery.CountAsync(x => x.Status == BookCopyStatus.Available);

            var borrowBooks = await unitOfWork.LoanBooks.Query()
                .Where(x => x.BookCopy.Location.BranchId == request.Id
                && x.Status == LoanStatus.Active)
                .CountAsync();

            var totalFines = await unitOfWork.Fines.Query()
            .Where(x => x.LoanBook.BookCopy.Location.BranchId == request.Id)
            .SumAsync(x => x.TotalAmount);

            var mostBorrowed = await unitOfWork.LoanBooks.Query()
             .Where(l => l.BookCopy.Location.BranchId == request.Id)
             .GroupBy(l => l.BookCopy.Book.Title)
             .Select(g => new
             {
                 Title = g.Key,
                 Count = g.Count()
             })
             .OrderByDescending(x => x.Count)
             .FirstOrDefaultAsync(cancellationToken);

            var branchStatisticsDto = new BranchStatisticsDto
            {
                BranchId = request.Id,
                BranchName = branch.Name,
                TotalBooks = totalBooks,
                AvailableBooks = availableBooks,
                BorrowedBooks = borrowBooks,
                TotalFines = totalFines,
                MostBorrowedBookCount = mostBorrowed!.Count,
                MostBorrowedBookTitle = mostBorrowed.Title,
            };
            return Result<BranchStatisticsDto>.Success(branchStatisticsDto);

        }
    }
}
