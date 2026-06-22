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

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetMyLateBooks
{
    public class GetMyLateBooksQueryHandler : IRequestHandler<GetMyLateBooksQuery, Result<List<MyLateBookDetailDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetMyLateBooksQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<List<MyLateBookDetailDto>>> Handle(GetMyLateBooksQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<List<MyLateBookDetailDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated."
                );

            var userId = currentUserService.UserId!;

            var lateBooks = await unitOfWork.LoanBooks.Query()
              .AsNoTracking()
              .Where(x => DateTime.Now > x.DueDate && x.ReturnDate == null
              && x.UserId == userId && x.Status != Data.Enum.LoanStatus.Lost)
              .OrderByDescending(x => x.LoanDate)
              .Select(x => new MyLateBookDetailDto
              {

                  LoanDate = x.LoanDate,
                  DueDate = x.DueDate,
                  BookCopyId = x.BookCopyId,

                  BookTitle = x.BookCopy.Book.Title,

              }).ToListAsync();

            return Result<List<MyLateBookDetailDto>>.Success(lateBooks);
        }
    }
}
