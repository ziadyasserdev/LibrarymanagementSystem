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

namespace LibrarymanagementSystem.Application.Features.Publishers.Queries.GetPublisherBooksCount
{
    public class GetPublisherBooksCountQueryHandler : IRequestHandler<GetPublisherBooksCountQuery, Result<List<PublisherBooksCountDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetPublisherBooksCountQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<PublisherBooksCountDto>>> Handle(GetPublisherBooksCountQuery request, CancellationToken cancellationToken)
        {
            var publisherDto = await unitOfWork.Publishers.Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .Select( x => new PublisherBooksCountDto
                {
                    PublisherId=x.Id,
                    PublisherName=x.Name,
                    BooksCount= x.Books.Count,
                }).ToListAsync();
            return Result<List<PublisherBooksCountDto>>.Success(publisherDto);
        }
    }
}
