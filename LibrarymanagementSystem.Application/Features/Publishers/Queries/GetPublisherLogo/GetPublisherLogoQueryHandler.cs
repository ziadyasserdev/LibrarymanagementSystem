using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Contracts.Services;
using LibrarymanagementSystem.Application.Features.Publishers.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Queries.GetPublisherLogo
{
    public class GetPublisherLogoQueryHandler : IRequestHandler<GetPublisherLogoQuery, Result<PublisherLogoDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileService fileService;

        public GetPublisherLogoQueryHandler(IUnitOfWork unitOfWork,IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.fileService = fileService;
        }
        public async Task<Result<PublisherLogoDto>> Handle(GetPublisherLogoQuery request, CancellationToken cancellationToken)
        {
            var publisher = await unitOfWork.Publishers.Query()
             .FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted);
            if (publisher == null)
                return Result<PublisherLogoDto>.Failure(ResultStatus.NotFound, "Publisher not found");
            var result = await fileService.DownloadFileAsync(publisher.LogoUrl!);
            var publisherLogoDto = new PublisherLogoDto
            {
                Content=result.Value!,
                ContentType= "image/png"
            };
            return Result<PublisherLogoDto>.Success(publisherLogoDto);
        }
    }
}
