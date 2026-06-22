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

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetMyDamagedBooks
{
    public class GetMyDamagedBooksQueryHandler : IRequestHandler<GetMyDamagedBooksQuery, Result<List<DamageBookDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetMyDamagedBooksQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<List<DamageBookDto>>> Handle(GetMyDamagedBooksQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<List<DamageBookDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated."
                );

            var userId = currentUserService.UserId!;
            var query = await unitOfWork.LoanBooks.Query()
                .AsNoTracking()
                .Where(x => x.Fine != null && x.UserId == userId
                && x.Fine.FineType == Data.Enum.FineType.DamagedBook)
                .Select(x => new DamageBookDto
                {
                    Id = x.Id,
                    
                    BookCopyId= x.BookCopyId,
                    BookTitle = x.BookCopy.Book.Title,
                    ISBN = x.BookCopy.Book.ISBN,
                    UserId = x.UserId,
                    UserFullName = x.User.FullName,
                    DamageDate = x.ReturnDate,
                    Notes = x.Fine!.Notes
                }).ToListAsync();

            return Result<List<DamageBookDto>>.Success(query);
        }
    }
}
