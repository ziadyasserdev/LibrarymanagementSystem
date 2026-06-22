using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Commands.DeletePublisher
{
    public class DeletePublisherCommandHandler : IRequestHandler<DeletePublisherCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public DeletePublisherCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(DeletePublisherCommand request, CancellationToken cancellationToken)
        {
            var publisher = await unitOfWork.Publishers.GetByIdAsync(request.Id);
            if (publisher == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Publisher not found");
            if (publisher.IsDeleted)
                return Result<int>.Failure(ResultStatus.Conflict, "Publisher already inactive");
            publisher.IsDeleted = true;
            publisher.IsActive = false;
            publisher.UpdatedAt = DateTime.Now;
            publisher.DeletedAt = DateTime.Now;
            publisher.DeletedBy = currentUserService.UserName ?? "Default Admin";
            await unitOfWork.SaveAsync();
            return Result<int>.Success(publisher.Id);
        }
    }
}
