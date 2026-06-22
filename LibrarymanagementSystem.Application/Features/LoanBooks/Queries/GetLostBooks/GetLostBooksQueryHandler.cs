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

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetLostBooks
{
    public class GetLostBooksQueryHandler : IRequestHandler<GetLostBooksQuery, Result<List<LostBookDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetLostBooksQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<List<LostBookDto>>> Handle(GetLostBooksQuery request, CancellationToken cancellationToken)
        {
            var lostBooks = await unitOfWork.LoanBooks.Query()
                .AsNoTracking()
                .Where(x => x.Status == LoanStatus.Lost)
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

            return Result<List<LostBookDto>>.Success(lostBooks);
        }
    }
}
