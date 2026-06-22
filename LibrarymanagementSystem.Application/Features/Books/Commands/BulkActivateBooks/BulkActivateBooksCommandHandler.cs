using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Commands.BulkActivateBooks
{
    public class BulkActivateBooksCommandHandler : IRequestHandler<BulkActivateBooksCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public BulkActivateBooksCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(BulkActivateBooksCommand request, CancellationToken cancellationToken)
        {
            var BookIds = request.BookIds.Distinct().ToList();
            var books = await unitOfWork.Books.Query()
                .Where(x => BookIds.Contains(x.Id) && !x.IsDeleted)
                .ToListAsync();
            if(!books.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No books found for the provided IDs");
            var booksToActivate=books.Where(x => !x.IsActive).ToList();
            if(!booksToActivate.Any())
                return Result<string>.Failure(ResultStatus.Failure, "All provided books are already active");
            foreach(var book in booksToActivate)
            {
                book.IsActive = true;
                book.UpdatedAt = DateTime.Now;
            }
            await unitOfWork.SaveAsync();
            return Result<string>.Success($"{booksToActivate.Count} book(s) activated successfully");
        }
    }
}
