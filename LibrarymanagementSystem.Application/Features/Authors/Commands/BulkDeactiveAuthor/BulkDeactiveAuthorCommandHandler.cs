using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Commands.BulkDeactiveAuthor
{
    public class BulkDeactiveAuthorCommandHandler : IRequestHandler<BulkDeactiveAuthorCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public BulkDeactiveAuthorCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(BulkDeactiveAuthorCommand request, CancellationToken cancellationToken)
        {
            var authorIds = request.AuthorIds.Distinct().ToList();

            var authorsToDectivate = await unitOfWork.Authors.Query()
                .Where(x => authorIds.Contains(x.AuthorId) && !x.IsDeleted && x.IsActive)
                .ToListAsync(cancellationToken);

            if (!authorsToDectivate.Any())
                return Result<string>.Failure(ResultStatus.Failure, "All authors are already deactive or not found");

            foreach (var author in authorsToDectivate)
            {
                author.IsActive = false;
                author.UpdatedAt = DateTime.Now;
            }

            await unitOfWork.SaveAsync();
            return Result<string>.Success($"{authorsToDectivate.Count} Authors deactivated successfully");
        }
    }
}
