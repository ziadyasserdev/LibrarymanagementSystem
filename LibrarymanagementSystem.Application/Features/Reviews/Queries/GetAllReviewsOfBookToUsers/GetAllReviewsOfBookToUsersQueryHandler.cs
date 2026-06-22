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

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.GetAllReviewsOfBookToUsers
{
    public class GetAllReviewsOfBookToUsersQueryHandler : IRequestHandler<GetAllReviewsOfBookToUsersQuery, Result<List<PublicReviewDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetAllReviewsOfBookToUsersQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
       
        public async Task<Result<List<PublicReviewDto>>> Handle(GetAllReviewsOfBookToUsersQuery request, CancellationToken cancellationToken)
        {
            var reviews = await unitOfWork.Reviews.Query()
                 .Where(x => x.BookId == request.Id && !x.IsDeleted)
                 .Select(x => new PublicReviewDto
                 {
                     UserName=x.User!.FullName,
                     Rating=x.Rating,
                     Comment=x.Comment,
                     CreatedAt=x.CreatedAt,
                 }).ToListAsync();
            return Result<List<PublicReviewDto>>.Success(reviews);
        }
    }
}
