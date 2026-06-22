using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.LoanBooks.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetReturnLoanBooks
{
    public class GetReturnLoanBookQueryHandler : IRequestHandler<GetReturnLoanBookQuery, Result<List<LoanBookDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetReturnLoanBookQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<LoanBookDto>>> Handle(GetReturnLoanBookQuery request, CancellationToken cancellationToken)
        {
            var returnBooks = await unitOfWork.LoanBooks.GetREturnBooks();
            if (returnBooks == null || !returnBooks.Any())
                return Result<List<LoanBookDto>>.Failure(ResultStatus.NotFound, "No returned books found.");
            var loanBooksDto = returnBooks.Select(lb => new LoanBookDto
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
