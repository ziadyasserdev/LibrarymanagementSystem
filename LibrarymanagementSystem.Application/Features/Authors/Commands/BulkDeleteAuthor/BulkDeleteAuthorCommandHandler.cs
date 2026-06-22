using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Commands.BulkDeleteAuthor
{
    public class BulkDeleteAuthorCommandHandler : IRequestHandler<BulkDeleteAuthorCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public BulkDeleteAuthorCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(BulkDeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            var AuthorIds = request.AuthorIds.ToList();
            var authors=await unitOfWork.Authors.Query()
                .Where(x => AuthorIds.Contains(x.AuthorId) && !x.IsDeleted)
                .ToListAsync(cancellationToken);
            if(!authors.Any())
                return Result<string>.Failure(ResultStatus.Failure, "All authors are already deleted or not found");
            var authorsWithBooks = await unitOfWork.Books.Query()
                .Where(b => AuthorIds.Contains(b.AuthorId) && !b.IsDeleted)
                .Select(b => b.AuthorId)
                .Distinct()
                .ToListAsync(cancellationToken);
           
            foreach(var author in authors)
            {
                if(authorsWithBooks.Contains(author.AuthorId))
                    return Result<string>.Failure(ResultStatus.Conflict, $"Cannot delete author with active books. AuthorId: {author.AuthorId}");
                author.IsDeleted = true;
                author.IsActive = false;
                author.DeletedAt = DateTime.Now;    
                author.UpdatedAt = DateTime.Now;
            }
            await unitOfWork.SaveAsync();
            return Result<string>.Success($"{authors.Count} Authors deleted successfully");
        }
    }
}
