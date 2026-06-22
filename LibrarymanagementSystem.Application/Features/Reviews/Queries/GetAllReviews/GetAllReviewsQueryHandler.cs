using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Reviews.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.GetAllReviews
{
    public class GetAllReviewsQueryHandler : IRequestHandler<GetAllReviewsQuery, Result<List<AdminReviewDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllReviewsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<AdminReviewDto>>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
        {
            var reviews = await unitOfWork.Reviews.Query()
                .Select(x => new AdminReviewDto
                {
                    Id = x.Id,
                    BookId = x.BookId,
                    BookTitle = x.Book!.Title,
                    UserId = x.UserId,
                    UserName = x.User!.FullName,
                    Rating = x.Rating,
                    Comment = x.Comment,
                    IsDeleted = x.IsDeleted,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                })
                .ToListAsync();
          
            return Result<List<AdminReviewDto>>.Success(reviews);
        }
    }
}
