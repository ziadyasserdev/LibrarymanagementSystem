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

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.GetMyReviewOfSpecificBook
{
    public class GetMyReviewOfSpecificBookQueryHandler : IRequestHandler<GetMyReviewOfSpecificBookQuery, Result<MyReviewDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetMyReviewOfSpecificBookQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<MyReviewDto>> Handle(GetMyReviewOfSpecificBookQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<MyReviewDto>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated."
                );

            var userId = currentUserService.UserId!;

            var review = await unitOfWork.Reviews.Query()
                .Where(x => x.UserId == userId 
                && x.BookId == request.BookId 
                && !x.IsDeleted)
                .Select(x => new MyReviewDto
                {
                    Id = x.Id,
                    BookTitle = x.Book!.Title,
                    Rating = x.Rating,
                    Comment = x.Comment,
                    CreatedAt = x.CreatedAt
                }).FirstOrDefaultAsync();
            if (review is null)
                return Result<MyReviewDto>.Failure(
                    ResultStatus.NotFound,
                    "Review not found for this book."
                );

            return Result<MyReviewDto>.Success(review);
           
        }
    }
}
