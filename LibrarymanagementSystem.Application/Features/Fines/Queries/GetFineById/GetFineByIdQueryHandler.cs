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

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetFineById
{
    public class GetFineByIdQueryHandler : IRequestHandler<GetFineByIdQuery, Result<FineDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetFineByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<FineDto>> Handle(GetFineByIdQuery request, CancellationToken cancellationToken)
        {

            var fineDto = await unitOfWork.Fines.Query()
    .AsNoTracking()
    .Where(x => x.Id == request.Id)
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
    .FirstOrDefaultAsync();
            if (fineDto == null)
                return Result<FineDto>.Failure(ResultStatus.NotFound, "Fine not found");
            return Result<FineDto>.Success(fineDto);
        }
    }
}
