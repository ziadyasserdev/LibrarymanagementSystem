using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.RestoreReview
{
    public class RestoreReviewQueryHandler : IRequestHandler<RestoreReviewQuery, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public RestoreReviewQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(RestoreReviewQuery request, CancellationToken cancellationToken)
        {
            var review=await unitOfWork.Reviews.Query()
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            if (review is null)
               return Result<int>.Failure(ResultStatus.NotFound, "Review not found");
            if(!review.IsDeleted)
                return Result<int>.Failure(ResultStatus.Conflict, "Review already active");
            review.IsDeleted = false;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(review.Id);
        }
    }
}
