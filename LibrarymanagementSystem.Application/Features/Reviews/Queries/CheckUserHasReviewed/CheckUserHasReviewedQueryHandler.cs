using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Reviews.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.CheckUserHasReviewed
{
    public class CheckUserHasReviewedQueryHandler : IRequestHandler<CheckUserHasReviewedQuery, Result<bool>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public CheckUserHasReviewedQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<bool>> Handle(CheckUserHasReviewedQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<bool>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated."
                );
            var userId =currentUserService.UserId;
            var check=await unitOfWork.Reviews.Query()
                .AnyAsync(x => x.UserId == userId
                && x.BookId == request.Id);

            return Result<bool>.Success(check);

        }
    }
}
