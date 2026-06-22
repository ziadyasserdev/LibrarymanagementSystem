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

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetMyLostBooks
{
    public class GetMyLostBooksQueryHandler : IRequestHandler<GetMyLostBooksQuery, Result<List<LostBookDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetMyLostBooksQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<LostBookDto>>> Handle(GetMyLostBooksQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<List<LostBookDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated."
                );



            var userId = currentUserService.UserId!;
            var query = await unitOfWork.LoanBooks.Query()
                .AsNoTracking()
                .Where(x => x.Status == Data.Enum.LoanStatus.Lost && x.UserId == userId)
                .Select(x => new LostBookDto
                {
                    Id = x.Id,
                    BookCopyId = x.BookCopyId,
                    BookTitle = x.BookCopy.Book.Title,
                    ISBN = x.BookCopy.Book.ISBN,
                    UserId = x.User.Id,
                    UserFullName = x.User.FullName,
                    LostDate = x.Fine != null ? x.Fine.CreatedAt : null,
                    Notes = x.Fine != null ? x.Fine.Notes : string.Empty
                }).ToListAsync();

            return Result<List<LostBookDto>>.Success(query);

        }
    }
}
