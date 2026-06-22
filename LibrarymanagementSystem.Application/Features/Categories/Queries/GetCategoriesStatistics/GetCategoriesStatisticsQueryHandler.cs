using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Categories.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Queries.GetCategoriesStatistics
{
    public class GetCategoriesStatisticsQueryHandler : IRequestHandler<GetCategoriesStatisticsQuery, Result<List<CategoryStatisticsDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetCategoriesStatisticsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<CategoryStatisticsDto>>> Handle(GetCategoriesStatisticsQuery request, CancellationToken cancellationToken)
        {
            var categories = await unitOfWork.Categories.Query()
            .Include(c => c.Books)
                .ThenInclude(b => b.Copies)
            .Include(c => c.Books)
                .ThenInclude(b => b.Reviews)
            .ToListAsync(cancellationToken);

            var result = categories.Select(category =>
            {
                var books = category.Books;

                var copies = books.SelectMany(b => b.Copies);

                // Top Books
                var topBooks = books
                    .Select(b => new TopBookDto
                    {
                        BookId = b.Id,
                        Title = b.Title,
                        BorrowCount = copies.Count(c => c.BookId == b.Id && c.Status == BookCopyStatus.Loaned)
                    })
                    .OrderByDescending(x => x.BorrowCount)
                    .Take(5)
                    .ToList();

                // Breakdown
                var booksByLanguage = books
                    .GroupBy(b => b.Language)
                    .ToDictionary(g => g.Key, g => g.Count());

                var booksByYear = books
                    .GroupBy(b => b.PublishedYear)
                    .ToDictionary(g => g.Key, g => g.Count());

                return new CategoryStatisticsDto
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name,
                    TotalBooks = books.Count,
                    ActiveBooks = books.Count(b => !b.IsDeleted),
                    DeletedBooks = books.Count(b => b.IsDeleted),
                    AvailableCopies = copies.Count(c => c.Status == BookCopyStatus.Available),
                    BorrowedCopies = copies.Count(c => c.Status == BookCopyStatus.Loaned),
                    LostCopies = copies.Count(c => c.Status == BookCopyStatus.Lost),
                    TopBooks = topBooks,
                    BooksByLanguage = booksByLanguage,
                    BooksByYear = booksByYear,
                    DistinctAuthors = books.Select(b => b.AuthorId).Distinct().Count(),
                    AveragePrice = books.Any() ? books.Average(b => b.Price) : null,
                    MinPrice = books.Any() ? books.Min(b => b.Price) : null,
                    MaxPrice = books.Any() ? books.Max(b => b.Price) : null,
                    AverageRating = books.SelectMany(b => b.Reviews).Any() ? books.SelectMany(b => b.Reviews).Average(r => r.Rating) : null,
                    TotalReviews = books.SelectMany(b => b.Reviews).Count(),
                    IsDeleted = category.IsDeleted,
                    CreatedAt = category.CreatedAt,
                    LastUpdated = category.UpdatedAt
                };
            }).ToList();

            return Result<List<CategoryStatisticsDto>>.Success(result);
        }
    }
}
