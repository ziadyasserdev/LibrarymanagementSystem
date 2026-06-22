using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Publishers.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LibrarymanagementSystem.Application.Features.Publishers.Queries.GetPublisherCount
{
    public class GetPublisherCountQueryHandler : IRequestHandler<GetPublisherCountQuery, Result<PublisherCountDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetPublisherCountQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<PublisherCountDto>> Handle(GetPublisherCountQuery request, CancellationToken cancellationToken)
        {
            var publisherCountDto = new PublisherCountDto
            {
                TotalPublishers = await unitOfWork.Publishers.Query()
                .CountAsync(),
                ActivePublishers=await unitOfWork.Publishers.Query()
                .CountAsync(x => x.IsActive && !x.IsDeleted),
                DeletedPublishers =
                await unitOfWork.Publishers.Query().CountAsync(x =>
                    x.IsDeleted),
                InactivePublishers =
                await unitOfWork.Publishers.Query().CountAsync(x =>
                    !x.IsActive && !x.IsDeleted),

            };
            return Result<PublisherCountDto>.Success(publisherCountDto);
        }
    }
}
