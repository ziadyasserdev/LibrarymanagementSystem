using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Branches.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Queries.GetBranchDetails
{
    public class GetBranchDetailsQueryHandler : IRequestHandler<GetBranchDetailsQuery, Result<BranchDetailsDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetBranchDetailsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<BranchDetailsDto>> Handle(GetBranchDetailsQuery request, CancellationToken cancellationToken)
        {
            var branchDetails = await unitOfWork.Branches.Query()
     .Where(b => b.Id == request.Id && !b.IsDeleted)
     .Include(b => b.Locations)
         .ThenInclude(l => l.BookCopies)
             .ThenInclude(book => book.LoanBooks)

     .AsNoTracking()
     .FirstOrDefaultAsync(cancellationToken);

            if (branchDetails == null)
                return Result<BranchDetailsDto>.Failure(ResultStatus.NotFound, "Branch not found");

            var dto = new BranchDetailsDto
            {
                Id = branchDetails.Id,
                Name = branchDetails.Name,
                IsActive = branchDetails.IsActive,
                BooksCount = branchDetails.Locations.Sum(l => l.BookCopies.Count(bk => !bk.IsLost)),

                LoansCount = branchDetails.Locations.Sum(l => l.BookCopies.Sum(bk => bk.LoanBooks.Count(lb => lb.Status == Data.Enum.LoanStatus.Active)))
            };

            return Result<BranchDetailsDto>.Success(dto);
        }
    }
}