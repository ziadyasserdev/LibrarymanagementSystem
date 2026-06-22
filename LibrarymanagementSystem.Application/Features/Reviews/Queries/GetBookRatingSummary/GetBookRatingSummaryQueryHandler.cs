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

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.GetBookRatingSummary
{
    public class GetBookRatingSummaryQueryHandler : IRequestHandler<GetBookRatingSummaryQuery, Result<BookRatingSummaryDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetBookRatingSummaryQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<BookRatingSummaryDto>> Handle(GetBookRatingSummaryQuery request, CancellationToken cancellationToken)
        {

            var bookExists = await unitOfWork.Books.Query()
                .AnyAsync(x => x.Id == request.Id);

            if (!bookExists)
                return Result<BookRatingSummaryDto>.Failure(ResultStatus.NotFound, "Book not found");

            var reviews =  unitOfWork.Reviews.Query()
                .Where(x => x.BookId == request.Id && !x.IsDeleted)
                .AsNoTracking();


            var totalReviews = await reviews.CountAsync(cancellationToken);

            double averageRating = 0;

            if (totalReviews > 0)
            {
                averageRating = await reviews
                    .AverageAsync(r => r.Rating, cancellationToken);

                averageRating = Math.Round(averageRating, 2);
            }

            var bookRateDto = new BookRatingSummaryDto
            {
                BookId = request.Id,
                AverageRating = averageRating,
                TotalReviews = totalReviews
            };

            return Result<BookRatingSummaryDto>.Success(bookRateDto);
        }
    }
}
