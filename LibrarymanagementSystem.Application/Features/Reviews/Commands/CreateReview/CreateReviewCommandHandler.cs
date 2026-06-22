using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.LoanBooks.Dtos;
using LibrarymanagementSystem.Application.Features.Reviews.Dtos;
using LibrarymanagementSystem.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Result<ReviewDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public CreateReviewCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<ReviewDto>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<ReviewDto>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated."
                );

            var userId = currentUserService.UserId!;
            var book = await unitOfWork.Books.Query()
                .FirstOrDefaultAsync(x => x.Id == request.BookId);

            if (book is null)
                return Result<ReviewDto>.Failure(ResultStatus.NotFound, "Book not found");

            var checkFines = await unitOfWork.Fines.Query()
               .AnyAsync(x => x.LoanBook.UserId == userId && x.PaidAmount < x.TotalAmount);

            if (checkFines)
                return Result<ReviewDto>.Failure(ResultStatus.Forbidden, "You cannot review books while you have unpaid fines.");

            var checkBorrowing = await unitOfWork.LoanBooks.Query()
                .AnyAsync(x => x.BookCopyId == request.BookId
                && x.UserId == userId);

            if (!checkBorrowing)
                return Result<ReviewDto>.Failure(ResultStatus.Forbidden, "You can't review book you don't borrow it");

            var checkDuplicate = await unitOfWork.Reviews.Query()
                .AnyAsync(x => x.UserId == userId
                && x.BookId == request.BookId
                && !x.IsDeleted);

            if (checkDuplicate)
                return Result<ReviewDto>.Failure(ResultStatus.Conflict, "You have already reviewed this book.");

            var review = new Review
            {
                Rating = request.Rating,
                Comment = request.Comment,
                BookId = request.BookId,
                UserId = userId,
                CreatedAt = DateTime.Now,
            };


            await unitOfWork.Reviews.AddAsync(review);
            await unitOfWork.SaveAsync();
            var reviewDto = new ReviewDto
            {
                Id = review.Id,
                Comment = request.Comment,
                Rating = request.Rating,
                CreatedAt = review.CreatedAt,
                UserName = currentUserService.UserName!
            };

            return Result<ReviewDto>.Success(reviewDto);
        }
    }
}
