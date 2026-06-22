using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Commands.BulkActiveAuthor
{
    public class BulkActiveAuthorCommnandHandler : IRequestHandler<BulkActiveAuthorCommnand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public BulkActiveAuthorCommnandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(BulkActiveAuthorCommnand request, CancellationToken cancellationToken)
        {
            var authorIds = request.AuthorIds.Distinct().ToList();

            var authorsToActivate = await unitOfWork.Authors.Query()
                .Where(x => authorIds.Contains(x.AuthorId) && !x.IsDeleted && !x.IsActive)
                .ToListAsync(cancellationToken);

            if (!authorsToActivate.Any())
                return Result<string>.Failure(ResultStatus.Failure, "All authors are already active or not found");

            foreach (var author in authorsToActivate)
            {
                author.IsActive = true;
                author.UpdatedAt = DateTime.Now;
            }

            await unitOfWork.SaveAsync();
            return Result<string>.Success($"{authorsToActivate.Count} Authors activated successfully");
        }
    }
}
