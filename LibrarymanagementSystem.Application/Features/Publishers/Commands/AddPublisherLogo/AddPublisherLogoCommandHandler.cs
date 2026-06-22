using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Contracts.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Commands.AddPublisherLogo
{
    public class AddPublisherLogoCommandHandler : IRequestHandler<AddPublisherLogoCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileService fileService;

        public AddPublisherLogoCommandHandler(IUnitOfWork unitOfWork,IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.fileService = fileService;
        }
        public async Task<Result<int>> Handle(AddPublisherLogoCommand request, CancellationToken cancellationToken)
        {
            var publisher=await unitOfWork.Publishers.Query()
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            if (publisher == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Publisher not found");
            var result = await fileService.UploadImageAsync(request.formFile);
            if (!result.IsSuccess)
                return Result<int>.Failure(ResultStatus.Failure, result.Error);
            publisher.LogoUrl = result.Value!.Url;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(publisher.Id);
        }
    }
}
