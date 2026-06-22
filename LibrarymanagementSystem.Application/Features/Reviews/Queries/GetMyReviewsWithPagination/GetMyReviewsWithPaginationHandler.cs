using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Reviews.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.GetMyReviewsWithPagination
{
    public class GetMyReviewsWithPaginationQueryHandler : IRequestHandler<GetMyReviewsWithPaginationQuery, Result<PaginatedResult<MyReviewDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetMyReviewsWithPaginationQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<PaginatedResult<MyReviewDto>>> Handle(GetMyReviewsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<PaginatedResult<MyReviewDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated."
                );
            var userId = currentUserService.UserId!;
            var query = unitOfWork.Reviews.Query()
                .Where(x => x.UserId == userId && !x.IsDeleted);
            switch(request.ReviewFilter)
            {
                case ReviewFilter.Rating:
                    if (request.OrderByDescending == true)
                        query = query.OrderByDescending(x => x.Rating);
                    else
                        query = query.OrderBy(x => x.Rating);
                    break;
                case ReviewFilter.CreatedAt:
                    if (request.OrderByDescending == true)
                        query = query.OrderByDescending(x => x.CreatedAt);
                    else
                        query = query.OrderBy(x => x.CreatedAt);
                    break;
            }
            var totalCount=await query.CountAsync();
            var items=await query.Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new MyReviewDto
                {
                    Id = x.Id,
                    BookTitle = x.Book!.Title,
                    Rating = x.Rating,
                    Comment = x.Comment,
                    CreatedAt = x.CreatedAt
                }).ToListAsync();

            return Result<PaginatedResult<MyReviewDto>>.Success(new PaginatedResult<MyReviewDto>(items, request.PageNumber, request.PageSize, totalCount));
        }
    }
}
