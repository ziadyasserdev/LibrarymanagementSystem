using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Authors.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorStatistics
{
    public class GetAuthorStatisticsQueryHandler : IRequestHandler<GetAuthorStatisticsQuery, Result<AuthorStatisticsDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAuthorStatisticsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
  
        public async Task<Result<AuthorStatisticsDto>> Handle(GetAuthorStatisticsQuery request, CancellationToken cancellationToken)
        {
            var author = await unitOfWork.Authors.Query()
                .Include(a => a.Books)
                    .ThenInclude(b => b.Reviews)
                .Include(a => a.Books)
                .ThenInclude(x => x.Copies)
                .ThenInclude(x => x.LoanBooks)
                .FirstOrDefaultAsync(a => a.AuthorId == request.AuthorId
                && !a.IsDeleted
                , cancellationToken);

            if (author == null)
                return Result<AuthorStatisticsDto>.Failure(ResultStatus.NotFound, "Author not found or deleted");

            var dto = new AuthorStatisticsDto
            {
                AuthorId= author.AuthorId,
                Name=author.Name,
                IsDeleted=author.IsDeleted,
                IsActive=author.IsActive,
                CreatedAt=author.CreatedAt,
                UpdatedAt=author.UpdatedAt,
                DeletedAt=author.DeletedAt,
                TotalBooks =author.Books.Count,   
                ActiveBooks=author.Books.Count(b=>!b.IsDeleted && b.IsActive),
                DeletedBooks=author.Books.Count(x => x.IsDeleted),
                TotalCopies=author.Books.Count(b => b.Copies.Any()),
                AvailableCopies=author.Books.Sum(b => b.Copies.Count(c => c.Status == BookCopyStatus.Available && !c.IsDeleted)),
                BorrowedCopies=author.Books.Sum(b => b.Copies.Count(c => c.Status == BookCopyStatus.Loaned && !c.IsDeleted)),
                ReservedCopies=author.Books.Sum(b => b.Copies.Count(c => c.Status == BookCopyStatus.Reserved && !c.IsDeleted)),
                LostCopies=author.Books.Sum(b => b.Copies.Count(c => c.Status == BookCopyStatus.Lost && !c.IsDeleted)),
                AverageRating=author.Books.Where(b => b.Reviews.Any()).Average(b => b.Reviews.Average(r => r.Rating)),
                TotalReviews=author.Books.Sum(b => b.Reviews.Count),
                BooksByCategory=author.Books
                .GroupBy(x => x.CategoryId)
                .ToDictionary(g => g.Key, g => g.Count()),
                BooksByYear = author.Books
                .GroupBy(x => x.PublishedYear)
                .ToDictionary(g => g.Key, g => g.Count()),
                BooksByLanguage= author.Books
                .GroupBy(x => x.Language)
                .ToDictionary(g => g.Key, g => g.Count()),
                TopBooks=author.Books.OrderByDescending(x => x.Price)
                .Take(3).Select(x => new TopBookDto
                {
                    BookId = x.Id,
                    Title = x.Title,
                    BorrowCount = x.Copies.Count(c => c.Status == BookCopyStatus.Loaned && !c.IsDeleted),
                    AverageRating = x.Reviews.Any() ? x.Reviews.Average(r => r.Rating) : (double?)null
                }).ToList(),

            
            };
            var bookIds = author.Books.Select(b => b.Id).ToList();

            var loanQuery = unitOfWork.LoanBooks.Query()
                .Where(l => bookIds.Contains(l.BookCopy.BookId));
            var loanHistory = await loanQuery
    .GroupBy(l => new { l.BookCopy.BookId, l.BookCopy.Book.Title })
    .Select(g => new LoanHistoryDto
    {
        BookId = g.Key.BookId,
        BookTitle = g.Key.Title,
        LoanedCount = g.Count(),
        ReturnedCount = g.Count(x => x.ReturnDate != null),
        OverdueCount = g.Count(x => x.ReturnDate == null && DateTime.Now > x.DueDate)
    })
    .ToListAsync(cancellationToken);
            dto.LoanHistory= loanHistory;
            return Result<AuthorStatisticsDto>.Success(dto);
        }
    }
}
