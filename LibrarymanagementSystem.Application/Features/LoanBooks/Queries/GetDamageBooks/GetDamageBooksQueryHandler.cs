using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.LoanBooks.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetDamageBooks
{
    public class GetDamageBooksQueryHandler : IRequestHandler<GetDamageBooksQuery, Result<List<DamageBookDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetDamageBooksQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<DamageBookDto>>> Handle(GetDamageBooksQuery request, CancellationToken cancellationToken)
        {
            var damageBooks = await unitOfWork.LoanBooks.Query()
      .Where(x => x.Fine != null
               && x.Fine.FineType == FineType.DamagedBook)
      .Select(x => new DamageBookDto
      {
          Id = x.Id,
          BookCopyId = x.BookCopyId,
          BookTitle = x.BookCopy.Book.Title,
          ISBN = x.BookCopy.Book.ISBN,
          UserId = x.UserId,
          UserFullName = x.User.FullName,
          DamageDate = x.ReturnDate,
          Notes = x.Fine.Notes
      })
      .ToListAsync(cancellationToken);

            return Result<List<DamageBookDto>>.Success(damageBooks);
        }
    }
}
