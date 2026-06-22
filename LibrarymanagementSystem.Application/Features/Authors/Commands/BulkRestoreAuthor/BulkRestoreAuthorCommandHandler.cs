using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Commands.BulkRestoreAuthor
{
    public class BulkRestoreAuthorCommandHandler : IRequestHandler<BulkRestoreAuthorCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public BulkRestoreAuthorCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(BulkRestoreAuthorCommand request, CancellationToken cancellationToken)
        {
            var AuthorIds = request.AuthorIds.ToList();
            var authors = await unitOfWork.Authors.Query()
                .Where(x => AuthorIds.Contains(x.AuthorId) && x.IsDeleted)
                .ToListAsync(cancellationToken);
            if (!authors.Any())
                return Result<string>.Failure(ResultStatus.Failure, "All authors are already notdeleted or not found");
            var activeAuthorNames = await unitOfWork.Authors.Query()
     .Where(x => !x.IsDeleted)
     .Select(x => x.Name)
     .ToListAsync(cancellationToken);
            foreach (var author in authors)
            {
                if (activeAuthorNames.Contains(author.Name))
                    return Result<string>.Failure(ResultStatus.Conflict, $"Cannot restore author with duplicate name. Author Name: {author.Name}");

                author.IsDeleted = false;
                author.DeletedAt =null;
                author.UpdatedAt = DateTime.Now;
            }
            await unitOfWork.SaveAsync();
            return Result<string>.Success($"{authors.Count} Authors restored successfully");
        }
    }
}
