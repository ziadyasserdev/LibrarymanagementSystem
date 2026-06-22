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

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.GetMyReviews
{
    public class GetMyReviewsQueryHandler : IRequestHandler<GetMyReviewsQuery, Result<List<MyReviewDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetMyReviewsQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
     
        public async Task<Result<List<MyReviewDto>>> Handle(GetMyReviewsQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<List<MyReviewDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated."
                );

            var userId = currentUserService.UserId!;
            var myReviews = await unitOfWork.Reviews.Query()
                .Where(x => x.UserId == userId && !x.IsDeleted)
                .Select(x => new MyReviewDto
                {
                    Id=x.Id,
                    BookTitle=x.Book!.Title,
                    Rating=x.Rating,
                    Comment=x.Comment,
                    CreatedAt=x.CreatedAt
                }).ToListAsync();

            return Result<List<MyReviewDto>>.Success(myReviews);
        }
    }
}
