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

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.GetReviewById
{
    public class GetReviewByIdQueryHandler : IRequestHandler<GetReviewByIdQuery, Result<AdminReviewDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetReviewByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<AdminReviewDto>> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            var review =await  unitOfWork.Reviews.Query()
                .Where(x => x.Id == request.Id)
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
                }).FirstOrDefaultAsync() ;

            if (review is null)
                return Result<AdminReviewDto>.Failure(ResultStatus.NotFound, "Review not found");
            return Result<AdminReviewDto>.Success(review);

        }
    }
}
