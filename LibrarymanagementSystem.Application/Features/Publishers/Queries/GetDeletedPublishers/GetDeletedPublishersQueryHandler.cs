using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Publishers.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Queries.GetDeletedPublishers
{
    public class GetDeletedPublishersQueryHandler : IRequestHandler<GetDeletedPublishersQuery, Result<List<DeletedPublisherDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetDeletedPublishersQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<DeletedPublisherDto>>> Handle(GetDeletedPublishersQuery request, CancellationToken cancellationToken)
        {
           var publishers=await unitOfWork.Publishers.Query()
                .AsNoTracking()
                .Where(x => x.IsDeleted)
                .Select(x => new DeletedPublisherDto
                {
                    Id=x.Id,
                    Name=x.Name,
                    DeletedBy=x.DeletedBy,
                    DeletedDate=x.DeletedAt
                }).ToListAsync();
            return Result<List<DeletedPublisherDto>>.Success(publishers);
        }
    }
}
