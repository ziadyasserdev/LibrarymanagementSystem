using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetAdvancedBookStatistics
{
    public class GetAdvancedBookStatisticsQueryHandler : IRequestHandler<GetAdvancedBookStatisticsQuery, Result<BookStatisticsDto>>
    {
        public readonly IUnitOfWork unitOfWork;

        public GetAdvancedBookStatisticsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<BookStatisticsDto>> Handle(GetAdvancedBookStatisticsQuery request, CancellationToken cancellationToken)
        {
            var books = unitOfWork.Books.Query()
                   .Include(b => b.Author)
            .Include(b => b.Category)
            .Include(b => b.Publisher)
            .Include(b => b.Reviews);
            var totalBooks = await books.CountAsync(cancellationToken);
            var activeBooks = await books.CountAsync(b => b.IsActive && !b.IsDeleted, cancellationToken);
            var inactiveBooks = await books.CountAsync(b => !b.IsActive && !b.IsDeleted, cancellationToken);
            var deletedBooks = await books.CountAsync(b => b.IsDeleted, cancellationToken);

         
            var averagePrice = await books.Where(b => !b.IsDeleted).AverageAsync(b => (decimal?)b.Price, cancellationToken) ?? 0;
            var averagePages = await books.Where(b => !b.IsDeleted).AverageAsync(b => (double?)b.NumberOfPages, cancellationToken) ?? 0;
            var booksWithReviews = await books
          .Where(b => !b.IsDeleted)
          .Include(b => b.Reviews)
          .ToListAsync(cancellationToken);

      
            var averageRating = booksWithReviews
                .SelectMany(b => b.Reviews)
                .Select(r => (double)r.Rating)
                .DefaultIfEmpty(0)
                .Average();
            var booksPerCategory = await books
                .Where(b => !b.IsDeleted)
                .GroupBy(b => b.Category.Name)
                .Select(g => new AggregationDto
                {
                    Name = g.Key,
                    Count = g.Count(),
                    AveragePrice = g.Average(b => b.Price),
                    AveragePages = g.Average(b => b.NumberOfPages)
                })
                .ToListAsync(cancellationToken);

           
            var booksPerAuthor = await books
                .Where(b => !b.IsDeleted)
                .GroupBy(b => b.Author.Name)
                .Select(g => new AggregationDto
                {
                    Name = g.Key,
                    Count = g.Count(),
                    AveragePrice = g.Average(b => b.Price),
                    AveragePages = g.Average(b => b.NumberOfPages)
                })
                .ToListAsync(cancellationToken);

         
            var booksPerPublisher = await books
                .Where(b => !b.IsDeleted)
                .GroupBy(b => b.Publisher.Name)
                .Select(g => new AggregationDto
                {
                    Name = g.Key,
                    Count = g.Count(),
                    AveragePrice = g.Average(b => b.Price),
                    AveragePages = g.Average(b => b.NumberOfPages)
                })
                .ToListAsync(cancellationToken);
 
            var top5ExpensiveBooks = await books
                .Where(b => !b.IsDeleted)
                .OrderByDescending(b => b.Price)
                .Take(5)
                .Select(b => new AggregationDto
                {
                    Name = b.Title,
                    AveragePrice = b.Price,
                    AveragePages = b.NumberOfPages
                })
                .ToListAsync(cancellationToken);

         
            var top5BooksWithMostPages = await books
                .Where(b => !b.IsDeleted)
                .OrderByDescending(b => b.NumberOfPages)
                .Take(5)
                .Select(b => new AggregationDto
                {
                    Name = b.Title,
                    AveragePrice = b.Price,
                    AveragePages = b.NumberOfPages
                })
                .ToListAsync(cancellationToken);

            var stats = new BookStatisticsDto
            {
                TotalBooks =totalBooks,
                ActiveBooks = activeBooks,
                InactiveBooks = inactiveBooks,
                DeletedBooks = deletedBooks,
                AverageBookPrice = (double)averagePrice,
                AverageNumberOfPages = averagePages,
                AverageRating = averageRating,
                BooksPerCategory = booksPerCategory,
                BooksPerAuthor = booksPerAuthor,
                BooksPerPublisher = booksPerPublisher,
                Top5MostExpensiveBooks = top5ExpensiveBooks,
                Top5BooksWithMostPages = top5BooksWithMostPages
            };

            return Result<BookStatisticsDto>.Success(stats);


        }
    }
}
