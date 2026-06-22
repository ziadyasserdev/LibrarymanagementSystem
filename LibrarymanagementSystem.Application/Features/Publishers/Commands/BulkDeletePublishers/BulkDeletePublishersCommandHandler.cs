using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Commands.BulkDeletePublishers
{
    public class BulkDeletePublishersCommandHandler : IRequestHandler<BulkDeletePublishersCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public BulkDeletePublishersCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(BulkDeletePublishersCommand request, CancellationToken cancellationToken)
        {
            var publishers = await unitOfWork.Publishers.Query()
                .Where(x => request.PublisherIds.Contains(x.Id) && !x.IsDeleted)
                .ToListAsync();
            if (!publishers.Any())
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "No publishers found");
            foreach (var publisher in publishers)
            {
                publisher.IsDeleted = true;
                publisher.IsActive = false;
                publisher.UpdatedAt = DateTime.Now;
                publisher.DeletedAt= DateTime.Now;
                publisher.DeletedBy = currentUserService.UserName ?? "System";
            }
            await unitOfWork.SaveAsync();
            return Result<string>.Success("Publishers deleted successfully");
        }
    }
}
