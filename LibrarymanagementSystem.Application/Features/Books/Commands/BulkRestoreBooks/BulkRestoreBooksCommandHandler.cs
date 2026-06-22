using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.BulkRestoreBooks
{
    public class BulkRestoreBooksCommandHandler : IRequestHandler<BulkRestoreBooksCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public BulkRestoreBooksCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

       

        public async Task<Result<string>> Handle(BulkRestoreBooksCommand request, CancellationToken cancellationToken)
        {
            var BookIds = request.BookIds.Distinct().ToList();

            var books = await unitOfWork.Books.Query()
                .Where(x => BookIds.Contains(x.Id))
                .ToListAsync();

            if (!books.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No books found for the provided IDs");

            var booksToRestore = books.Where(x => x.IsDeleted).ToList();

            if (!booksToRestore.Any())
                return Result<string>.Failure(ResultStatus.Failure, "No books to restore, all provided books are already active");

            foreach (var book in booksToRestore)
            {
                if (await checkDuplicate(book.Title, book.AuthorId, book.Id))
                    return Result<string>.Failure(ResultStatus.Failure,
                        $"Cannot restore book with ID {book.Id} because it would create a duplicate with the same title and author");

                book.IsDeleted = false;
                book.DeletedAt = null;
                book.UpdatedAt = DateTime.Now;
            }

            await unitOfWork.SaveAsync();

            return Result<string>.Success($"{booksToRestore.Count} book(s) restored successfully");
        }
        private async Task<bool> checkDuplicate(string title, int authorId, int bookId)
        {
            return await unitOfWork.Books.Query()
                .AnyAsync(x =>
                    !x.IsDeleted &&
                    x.AuthorId == authorId &&
                    x.Id != bookId &&
                    EF.Functions.Like(x.Title, title)); 
        }

    }
}
   