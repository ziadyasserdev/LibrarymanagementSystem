using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Fines.Dtos;
using LibrarymanagementSystem.Data.Enum;
using LibrarymanagementSystem.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetAllFines
{
    public class GetAllFinesQueryHandler : IRequestHandler<GetAllFinesQuery, Result<List<FineDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllFinesQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<List<FineDto>>> Handle(GetAllFinesQuery request, CancellationToken cancellationToken)
        {
            var fines = await unitOfWork.Fines.Query()
     .Include(f => f.LoanBook)
         .ThenInclude(lb => lb.BookCopy)
         .ThenInclude(x => x.Book)
     .Include(f => f.LoanBook.User)
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
            return Result<List<FineDto>>.Success(fines);
        }
    }
}
