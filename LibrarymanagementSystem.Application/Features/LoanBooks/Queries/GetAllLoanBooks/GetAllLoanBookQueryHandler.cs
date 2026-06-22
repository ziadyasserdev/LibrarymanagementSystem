using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.LoanBooks.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetAllLoanBooks
{
    public class GetAllLoanBookQueryHandler : IRequestHandler<GetAllLoanBookQuery, Result<List<LoanBookDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllLoanBookQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<LoanBookDto>>> Handle(GetAllLoanBookQuery request, CancellationToken cancellationToken)
        {
            var loanBooks = await unitOfWork.LoanBooks.Query()
                .Include(x => x.User)
                .Include(x => x.BookCopy)
                .ThenInclude(x => x.Book)
                .ToListAsync();
            if (loanBooks == null || !loanBooks.Any())
                return Result<List<LoanBookDto>>.Failure(ResultStatus.NotFound, "No loan books found.");
            var loanBooksDto = loanBooks.Select(lb => new LoanBookDto
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
