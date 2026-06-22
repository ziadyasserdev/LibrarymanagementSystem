using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.LoanBooks.Dtos;
using LibrarymanagementSystem.Data.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetAllLateBooks
{
    public class GetAllLateBooksQueryHandler : IRequestHandler<GetAllLateBooksQuery, Result<List<LoanBookDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllLateBooksQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<LoanBookDto>>> Handle(GetAllLateBooksQuery request, CancellationToken cancellationToken)
        {
            var lateBooks = await unitOfWork.LoanBooks.GetLateBooks();
            if (lateBooks == null || !lateBooks.Any())
                return Result<List<LoanBookDto>>.Failure(ResultStatus.NotFound, "No late books found.");
            var loanBooksDto = lateBooks.Select(lb => new LoanBookDto
            {
                Id = lb.Id,
                LoanDate = lb.LoanDate,
                ReturnDate = lb.ReturnDate ?? DateTime.MinValue,
                DueDate = lb.DueDate,
                BookTitle = lb.BookCopy.Book.Title,
                UserName = lb.User.FullName,
                IsReturned = lb.ReturnDate.HasValue ? true : false
            }).ToList();

            return Result<List<LoanBookDto>>.Success(loanBooksDto);

        }
    }
}
