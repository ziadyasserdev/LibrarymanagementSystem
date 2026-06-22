using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.BulkDeleteBooks
{
    public class BulkDeleteBooksCommandHandler : IRequestHandler<BulkDeleteBooksCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public BulkDeleteBooksCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(BulkDeleteBooksCommand request, CancellationToken cancellationToken)
        {
            var BookIds = request.BookIds.Distinct().ToList();
            var books = await unitOfWork.Books.Query()
              .Where(x => BookIds.Contains(x.Id))
              .ToListAsync();

            if (!books.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No books found for the provided IDs");

            var booksToDelete=await unitOfWork.Books.Query()
                .Where(x => BookIds.Contains(x.Id) && !x.IsDeleted)
                .ToListAsync();
            if(!booksToDelete.Any())
                return Result<string>.Failure(ResultStatus.Conflict, "No books were found to delete.  all books are already deleted.");
            var booksWithCopies = await unitOfWork.Books.Query()
              .Where(b => BookIds.Contains(b.Id) && b.Copies.Any())
              .Select(b => b.Id)
              .ToListAsync(cancellationToken);

            if (booksWithCopies.Any())
            {
                var ids = string.Join(", ", booksWithCopies);
                return Result<string>.Failure(ResultStatus.Conflict,
                    $"Cannot delete the following book IDs because they have associated copies: {ids}");
            }
            foreach (var book in booksToDelete)
            {
                book.IsActive = false;
                book.IsDeleted = true;
                book.DeletedAt = DateTime.Now;
            }
            await unitOfWork.SaveAsync();
            return Result<string>.Success($"{booksToDelete.Count} book(s) deleted successfully");

        }
    }
}
