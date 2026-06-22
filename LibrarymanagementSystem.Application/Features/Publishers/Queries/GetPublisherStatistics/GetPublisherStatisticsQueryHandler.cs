using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Publishers.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Queries.GetPublisherStatistics
{
    public class GetPublisherStatisticsQueryHandler : IRequestHandler<GetPublisherStatisticsQuery, Result<PublisherStatisticsDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetPublisherStatisticsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<PublisherStatisticsDto>> Handle(GetPublisherStatisticsQuery request, CancellationToken cancellationToken)
        {
            var publisher = await unitOfWork.Publishers.Query()
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (publisher is null)
                return Result<PublisherStatisticsDto>
                    .Failure(ResultStatus.NotFound, "Publisher not found");

            var booksQuery = unitOfWork.Books.Query()
                .Where(x => x.PublisherId == request.Id);

            var reviewsQuery = unitOfWork.Reviews.Query()
                .Where(x => x.Book!.PublisherId == request.Id);

            var totalBooks = await booksQuery.CountAsync(cancellationToken);

            var totalReviews = await reviewsQuery.CountAsync(cancellationToken);

            var averageRating = totalReviews > 0
                ? Math.Round(await reviewsQuery.AverageAsync(x => x.Rating, cancellationToken), 2)
                : 0;

            var totalBorrowedBooks = await unitOfWork.LoanBooks.Query()
                .Where(x => x.BookCopy.Book.PublisherId == request.Id)
                .CountAsync(cancellationToken);

            var publisherStatisticsDto = new PublisherStatisticsDto
            {
                TotalBooks = totalBooks,
                TotalBorrowedBooks = totalBorrowedBooks,
                TotalReviews = totalReviews,
                AverageBookRating = averageRating
            };

            return Result<PublisherStatisticsDto>.Success(publisherStatisticsDto);

        }
    }
}
