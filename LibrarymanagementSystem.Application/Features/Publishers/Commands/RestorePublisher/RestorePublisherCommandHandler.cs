using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Commands.RestorePublisher
{
    public class RestorePublisherCommandHandler : IRequestHandler<RestorePublisherCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public RestorePublisherCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(RestorePublisherCommand request, CancellationToken cancellationToken)
        {
            var publisher = await unitOfWork.Publishers.GetByIdAsync(request.Id);
            if (publisher == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Publisher not found");
            if(!publisher.IsDeleted)
               return Result<int>.Failure(ResultStatus.Conflict, "Publisher already is active");
            publisher.IsDeleted = false;
            publisher.UpdatedAt= DateTime.Now;
            publisher.DeletedAt = null;
            publisher.DeletedBy = null;
            publisher.UpdatedBy = currentUserService.UserName??"Default Admin";
            await unitOfWork.SaveAsync();
           return Result<int>.Success(publisher.Id);
        }
    }
}
