using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.LoanBooks.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetMyLoanBooks
{
    public class GetMyLoanBooksQueryHandler : IRequestHandler<GetMyLoanBooksQuery, Result<List<MyLoanBookDetailDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetMyLoanBooksQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<List<MyLoanBookDetailDto>>> Handle(GetMyLoanBooksQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<List<MyLoanBookDetailDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated."
                );

            var userId = currentUserService.UserId!;
            var loanBooks = await unitOfWork.LoanBooks.Query()
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.LoanDate)
                .Select(x => new MyLoanBookDetailDto
                {

                    LoanDate = x.LoanDate,
                    DueDate = x.DueDate,
                    ReturnDate = x.ReturnDate,
                    BookTitle = x.BookCopy.Book.Title,
                    UserName = x.User.UserName!,
                    LoanStatus = x.Status,
                    IsReturned = x.ReturnDate.HasValue ? true : false,
                    HasFines = x.Fine != null && !x.Fine.IsPaid ? true : false,
                }).ToListAsync();

            return Result<List<MyLoanBookDetailDto>>.Success(loanBooks);
        }
    }
}
