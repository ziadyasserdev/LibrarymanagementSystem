using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.LoanBooks.Dtos;
using LibrarymanagementSystem.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetAllLateBooksByUserId
{
    public class GetAllLateBooksByUserIdQueryHandler : IRequestHandler<GetAllLateBooksByUserIdQuery, Result<List<LateBookDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public GetAllLateBooksByUserIdQueryHandler(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        public async Task<Result<List<LateBookDto>>> Handle(GetAllLateBooksByUserIdQuery request, CancellationToken cancellationToken)
        {
            if(await userManager.FindByIdAsync(request.UserId) is null)
                return Result<List<LateBookDto>>.Failure(ResultStatus.NotFound, "User not found");



            var lateBooks = await unitOfWork.LoanBooks.Query()
                .AsNoTracking()
             
                .Where(x => DateTime.Now > x.DueDate && x.ReturnDate == null
                            && x.UserId == request.UserId && x.Status != Data.Enum.LoanStatus.Lost)
                .OrderByDescending(x => x.LoanDate)
                .Select(x => new LateBookDto
                {
                    Id = x.Id,
                    LoanDate = x.LoanDate,
                    DueDate = x.DueDate,
                    BookCopyId = x.BookCopyId,
                    UserId = x.UserId,
                    BookTitle = x.BookCopy.Book.Title,
                    UserName = x.User.FullName,
                }).ToListAsync(cancellationToken);

            return Result<List<LateBookDto>>.Success(lateBooks);
        }
    }
}
