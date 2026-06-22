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

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetOverdueFines
{
    public class GetOverdueFinesQueryHandler : IRequestHandler<GetOverdueFinesQuery, Result<List<OverdueFineDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetOverdueFinesQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<List<OverdueFineDto>>> Handle(GetOverdueFinesQuery request, CancellationToken cancellationToken)
        {
            var fines = await unitOfWork.Fines.Query()
                .AsNoTracking()
                .Where(x => x.PaidAmount < x.TotalAmount && x.CreatedAt < DateTime.UtcNow)
                .Select(x => new OverdueFineDto
                {
                    FineId=x.Id,
                    LoanBookId=x.LoanBookId,
                    BookTitle=x.LoanBook.BookCopy.Book.Title,
                    DueDate=x.DueDate,
                    CreatedAt=x.CreatedAt,
                    IsPaid=x.IsPaid,
                    TotalAmount=x.TotalAmount,
                    UserName=x.LoanBook.User.UserName!

                }).ToListAsync(cancellationToken);
            return Result<List<OverdueFineDto>>.Success(fines);
        }
    }
}
