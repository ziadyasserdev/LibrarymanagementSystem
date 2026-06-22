using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.BulkDeactivateBooks
{
    public class BulkDeactivateBooksCommandHandler : IRequestHandler<BulkDeactivateBooksCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public BulkDeactivateBooksCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(BulkDeactivateBooksCommand request, CancellationToken cancellationToken)
        {
            var bookIds = request.BookIds.Distinct().ToList();

        
            var books = await unitOfWork.Books.Query()
                .Where(b => bookIds.Contains(b.Id) && !b.IsDeleted)
                .ToListAsync(cancellationToken);

            if (!books.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No books found for the provided IDs");

    
            var booksToDeactivate = books.Where(b => b.IsActive).ToList();
            if (!booksToDeactivate.Any())
                return Result<string>.Failure(ResultStatus.Failure, "All specified books are already inactive");

           
            var booksWithCopies = await unitOfWork.Books.Query()
                .Where(b => bookIds.Contains(b.Id) && b.Copies.Any())
                .Select(b => b.Id)
                .ToListAsync(cancellationToken);

            if (booksWithCopies.Any())
            {
                var ids = string.Join(", ", booksWithCopies);
                return Result<string>.Failure(ResultStatus.Conflict,
                    $"Cannot deactivate the following book IDs because they have associated copies: {ids}");
            }

          
            foreach (var book in booksToDeactivate)
            {
                book.IsActive = false;
                book.UpdatedAt = DateTime.UtcNow; 
            }

            await unitOfWork.SaveAsync();

            return Result<string>.Success($"{booksToDeactivate.Count} book(s) deactivated successfully");
        }
    }
}
