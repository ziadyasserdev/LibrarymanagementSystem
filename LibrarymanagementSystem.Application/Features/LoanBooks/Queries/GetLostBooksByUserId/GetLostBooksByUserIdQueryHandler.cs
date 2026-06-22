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

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetLostBooksByUserId
{
    public class GetDamageBooksQueryHandler : IRequestHandler<GetLateBooksByUserIdQuery, Result<List<LostBookDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public GetDamageBooksQueryHandler(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        public async Task<Result<List<LostBookDto>>> Handle(GetLateBooksByUserIdQuery request, CancellationToken cancellationToken)
        {
            if (await userManager.FindByIdAsync(request.UserId) is null)
                return Result<List<LostBookDto>>.Failure(ResultStatus.NotFound, "User not found");




            var query = await unitOfWork.LoanBooks.Query()
                 .AsNoTracking()
                 .Where(x => x.Status == LoanStatus.Lost && x.UserId == request.UserId)
                 .Select(x => new LostBookDto
                 {
                     Id = x.Id,
                     BookCopyId = x.BookCopyId,
                     BookTitle = x.BookCopy.Book.Title,
                     ISBN = x.BookCopy.Book.ISBN,
                     UserId = x.User.Id,
                     UserFullName = x.User.FullName,
                     LostDate = x.Fine!.CreatedAt,
                     Notes = x.Fine != null ? x.Fine.Notes : string.Empty
                 }).ToListAsync();

            return Result<List<LostBookDto>>.Success(query);
        }
        }
    }
