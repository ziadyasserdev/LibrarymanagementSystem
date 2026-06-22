using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Commands.DeleteFine
{
    public class DeleteFineCommandHandler : IRequestHandler<DeleteFineCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeleteFineCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(DeleteFineCommand request, CancellationToken cancellationToken)
        {
            var fine = await unitOfWork.Fines.Query()
              .Include(f => f.LoanBook)
              .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

            if (fine == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Fine not found.");

          
            if (fine.FineType == FineType.LateReturn || fine.FineType == FineType.LostBook)
                return Result<int>.Failure(ResultStatus.Conflict, "You cannot delete a system-generated fine.");

         
            if (fine.PaidAmount > 0)
                return Result<int>.Failure(ResultStatus.Conflict, "You cannot delete a fine that has payments.");

          
            unitOfWork.Fines.Delete(fine);
            await unitOfWork.SaveAsync();

           
            var hasOtherUnpaidFines = await unitOfWork.Fines.Query()
                .AnyAsync(f =>
                    f.LoanBookId == fine.LoanBookId &&
                    f.PaidAmount < f.TotalAmount,
                    cancellationToken);

            if (!hasOtherUnpaidFines && fine.LoanBook.ReturnDate.HasValue)
            {
               
                fine.LoanBook.Status = LoanStatus.Returned;
                await unitOfWork.SaveAsync();
            }
            else if (!hasOtherUnpaidFines && !fine.LoanBook.ReturnDate.HasValue)
            {
               
                fine.LoanBook.Status = LoanStatus.Active;
                await unitOfWork.SaveAsync();
            }

            return Result<int>.Success(fine.Id);

        }
    }
}
