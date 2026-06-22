using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Fines.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetAllFinesOfCurrentUser
{
    public class GetAllFinesOfCurrentUserQueryHandler : IRequestHandler<GetAllFinesOfCurrentUserQuery, Result<List<MyFineDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetAllFinesOfCurrentUserQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<MyFineDto>>> Handle(GetAllFinesOfCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            if (!currentUserService.IsAuthenticated)
                return Result<List<MyFineDto>>.Failure(ResultStatus.Unauthorized, "User is not authenticated.");
            var baseQuery = unitOfWork.Fines.Query();

            switch (request.FineStatus)
            {
                case FineStatus.UnPaid:
                    baseQuery = baseQuery.Where(x => x.PaidAmount < x.TotalAmount);
                    break;
                case FineStatus.Paid:
                    baseQuery = baseQuery.Where(x => x.PaidAmount == x.TotalAmount);
                    break;
            }
            var fines = await baseQuery
                .Include(c => c.LoanBook)
                .ThenInclude(x => x.BookCopy)
                .ThenInclude(x => x.Book)
                .Where(x => x.LoanBook.UserId == userId)
                       .Select(x => new MyFineDto
                       {
                           BookTitle = x.LoanBook.BookCopy.Book.Title,
                           FineType = (FineType)x.FineType,
                           TotalAmount = x.TotalAmount,
                           PaidAmount = x.PaidAmount,
                           CreatedAt = x.CreatedAt,
                           LoanDate = x.LoanBook.LoanDate
                       }).ToListAsync();

            if (!fines.Any())
                return Result<List<MyFineDto>>.Failure(ResultStatus.NotFound, "You donot have any fines");
            return Result<List<MyFineDto>>.Success(fines);
        }
    }
}
