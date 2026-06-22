using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Reviews.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.GetAllReviewsWithPagination
{
    public class GetAllReviewsWithPaginationQueryHandler : IRequestHandler<GetAllReviewsWithPaginationQuery, Result<PaginatedResult<AdminReviewDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllReviewsWithPaginationQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<PaginatedResult<AdminReviewDto>>> Handle(GetAllReviewsWithPaginationQuery request, CancellationToken cancellationToken)
        {

            var query =  unitOfWork.Reviews.Query();
            if(!string.IsNullOrWhiteSpace(request.UserId))
            {
                var checkUser=await unitOfWork.Reviews.Query()
                    .AnyAsync(r => r.UserId == request.UserId);
                if(!checkUser)
                    return Result<PaginatedResult<AdminReviewDto>>.Failure(ResultStatus.NotFound, "User not found");
                query = query.Where(x => x.UserId == request.UserId);
            }
                

            if (request.BookId.HasValue)
            {
                var checkBook = await unitOfWork.Reviews.Query()
                .AnyAsync(x => x.BookId == request.BookId);
                if (!checkBook)
                    return Result<PaginatedResult<AdminReviewDto>>.Failure(ResultStatus.NotFound, "Book not found");
                query = query.Where(x => x.BookId == request.BookId);
            }
              
           if(request.Rating.HasValue)
                query=query.Where(x => x.Rating == request.Rating.Value);

           if(request.IsDeleted.HasValue)
                query = query.Where(x => x.IsDeleted == request.IsDeleted.Value);

            query = request.OrderBy switch
            {
                "Rating" => request.OrderByDescending
                    ? query.OrderByDescending(r => r.Rating)
                    : query.OrderBy(r => r.Rating),
                _ => request.OrderByDescending
                    ? query.OrderByDescending(r => r.CreatedAt)
                    : query.OrderBy(r => r.CreatedAt),
            };

            var totalCount=await query.CountAsync();
            var items=await query.Skip((request.PageNumber-1) * request.PageSize)
                .Take(request.PageSize)
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
                }).ToListAsync();

            var paginatedResult=new PaginatedResult<AdminReviewDto>(items,request.PageNumber,request.PageSize,totalCount);
            return Result<PaginatedResult<AdminReviewDto>>.Success(paginatedResult);
        }
    }
}
