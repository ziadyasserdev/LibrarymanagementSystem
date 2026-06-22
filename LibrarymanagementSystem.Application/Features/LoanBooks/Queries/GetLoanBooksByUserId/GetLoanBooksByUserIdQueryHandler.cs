using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.LoanBooks.Dtos;
using LibrarymanagementSystem.Data.Enum;
using LibrarymanagementSystem.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetLoanBooksByUserId
{
    
    public class GetLoanBooksByUserIdQueryHandler : IRequestHandler<GetLoanBooksByUserIdQuery, Result<List<LoanBookDetailForAdminDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public GetLoanBooksByUserIdQueryHandler(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        public async Task<Result<List<LoanBookDetailForAdminDto>>> Handle(GetLoanBooksByUserIdQuery request, CancellationToken cancellationToken)
        {
            if (await userManager.FindByIdAsync(request.UserId) is null)
                return Result<List<LoanBookDetailForAdminDto>>.Failure(ResultStatus.NotFound, "User not found");





            var loanBooks = await unitOfWork.LoanBooks.Query()
                 .AsNoTracking()
                 .Where(x => x.UserId == request.UserId)
                 .OrderByDescending(x => x.LoanDate)
                 .Select(x => new LoanBookDetailForAdminDto
                 {
                     Id = x.Id,
                     LoanDate = x.LoanDate,
                     DueDate = x.DueDate,
                     ReturnDate = x.ReturnDate,
                     BookTitle = x.BookCopy.Book.Title,
                     UserName = x.User.UserName!,
                     LoanStatus = x.Status,
                     IsReturned = x.ReturnDate.HasValue ? true : false,
                     HasFines = x.Fine != null && !x.Fine.IsPaid ? true : false,

                 }).ToListAsync();

            return Result<List<LoanBookDetailForAdminDto>>.Success(loanBooks);
        }
    }
}
