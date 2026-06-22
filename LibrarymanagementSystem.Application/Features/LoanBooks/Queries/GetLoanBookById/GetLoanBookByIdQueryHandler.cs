using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.LoanBooks.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetLoanBookById
{
    public class GetLoanBookByIdQueryHandler : IRequestHandler<GetLoanBookByIdQuery, Result<LoanBookDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetLoanBookByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<LoanBookDto>> Handle(GetLoanBookByIdQuery request, CancellationToken cancellationToken)
        {
            var loanBook = await unitOfWork.LoanBooks
                .Query()
                .Include(i => i.BookCopy)
                .ThenInclude(x => x.Book)
                .Include(i => i.User)
                .FirstOrDefaultAsync(c => c.Id == request.Id);
            if (loanBook == null)
                return Result<LoanBookDto>.Failure(ResultStatus.NotFound, "Loan record not found.");

            var loanBookDto = new LoanBookDto
            {
                Id = loanBook.Id,
                LoanDate = loanBook.LoanDate,
                ReturnDate = loanBook.ReturnDate ?? DateTime.MinValue,
                DueDate = loanBook.DueDate,
                BookTitle = loanBook.BookCopy.Book.Title,
                UserName = loanBook.User.UserName!,
                IsReturned = loanBook.ReturnDate.HasValue ? true : false
            };
            return Result<LoanBookDto>.Success(loanBookDto);

        }
    }
}
