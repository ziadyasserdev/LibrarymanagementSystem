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

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetAllFinesByUserId
{
    public class GetAllFinesByUserIdQueryHandler : IRequestHandler<GetAllFinesByUserIdQuery, Result<List<FineDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllFinesByUserIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<List<FineDto>>> Handle(GetAllFinesByUserIdQuery request, CancellationToken cancellationToken)
        {

            var fines = await unitOfWork.Fines.Query()
                .Include(x => x.LoanBook)
                .ThenInclude(c => c.BookCopy)
                .ThenInclude(x => x.Book)
                .Where(x => x.LoanBook.UserId == request.UserId)
                .OrderByDescending(c => c.CreatedAt)
                .ThenBy(c => c.TotalAmount)
                .Select(f => new FineDto
                {
                    Id = f.Id,
                    LoanBookId = f.LoanBookId,
                    BookTitle = f.LoanBook.BookCopy.Book.Title,
                    UserId = f.LoanBook.UserId,
                    UserName = f.LoanBook.User.UserName!,
                    TotalAmount = f.TotalAmount,
                    PaidAmount = f.PaidAmount,
                    FineType = f.FineType,
                    CreatedAt = f.CreatedAt,
                    LoanStatus = f.LoanBook.Status
                })
                .ToListAsync();

            if (!fines.Any())
                return Result<List<FineDto>>.Failure(ResultStatus.NotFound, "No fines found to this user");
            return Result<List<FineDto>>.Success(fines);
        }
    }
}
