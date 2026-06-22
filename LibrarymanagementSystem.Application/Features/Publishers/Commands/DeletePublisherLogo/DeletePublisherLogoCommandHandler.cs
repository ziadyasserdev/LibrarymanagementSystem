using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Contracts.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Commands.DeletePublisherLogo
{
    public class DeletePublisherLogoCommandHandler : IRequestHandler<DeletePublisherLogoCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileService fileService;

        public DeletePublisherLogoCommandHandler(IUnitOfWork unitOfWork,IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.fileService = fileService;
        }
        public async Task<Result<int>> Handle(DeletePublisherLogoCommand request, CancellationToken cancellationToken)
        {
            var publisher = await unitOfWork.Publishers.Query()
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            if (publisher == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Publisher not found");
            if (publisher.LogoUrl != null)
            {
                var resultRemove = fileService.Remove(publisher.LogoUrl);
                if (!resultRemove.IsSuccess)
                    return Result<int>.Failure(ResultStatus.Failure, resultRemove.Error);
            }

            publisher.LogoUrl=null;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(publisher.Id);
        }
    }
}
