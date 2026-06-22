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

namespace LibrarymanagementSystem.Application.Features.Reviews.Commands.EditReview
{
    public class EditReviewCommandHandler : IRequestHandler<EditReviewCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public EditReviewCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(EditReviewCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<int>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated."
                );

            var userId = currentUserService.UserId!;

            var review = await unitOfWork.Reviews.Query()
              .FirstOrDefaultAsync(r => r.Id == request.Id);

            if (review is null || review.IsDeleted)
                return Result<int>.Failure(ResultStatus.NotFound, "Review not found or has been deleted.");

            if (review.UserId != userId)
                return Result<int>.Failure(ResultStatus.Forbidden, "You are not allowed to edit this review.");

            review.Rating=request.Rating;
            review.Comment=request.Comment ?? review.Comment;
            review.UpdatedAt=DateTime.Now;
            await unitOfWork.SaveAsync();

            return Result<int>.Success(review.Id);
        }
    }
}
