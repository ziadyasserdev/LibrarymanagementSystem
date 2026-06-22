using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Publishers.Dtos;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Queries.SearchByName
{
    public class SearchByNameQueryHandler : IRequestHandler<SearchByNameQuery, Result<List<PublisherReadDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public SearchByNameQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

        }

        public async Task<Result<List<PublisherReadDto>>> Handle(SearchByNameQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Publishers.Query().AsNoTracking();

            switch (request.publisherStatusFilter)
            {
                case PublisherStatusFilter.Active:
                    query = query.Where(c => c.IsActive);
                    break;
                case PublisherStatusFilter.Inactive:
                    query = query.Where(c => !c.IsActive);
                    break;
                case PublisherStatusFilter.All:
                default:
                    break;
            }
            if(!string.IsNullOrEmpty(request.name))

                query = query.Where(c => EF.Functions.Like(c.Name, $"%{request.name}%"));
            if (!string.IsNullOrEmpty(request.country))
                query = query.Where(c => EF.Functions.Like(c.Country, $"%{request.country}%"));
            if (!await query.AnyAsync())
                return Result<List<PublisherReadDto>>.Failure(ResultStatus.NotFound, "There are no publishers with this name");

            var result = await query
               
                .Select(c => new PublisherReadDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Phone = c.Phone,
                    Website = c.Website,
                    IsActive = c.IsActive,
                    CreatedAt = c.CreatedAt,
                    BookIds = c.Books.Select(b => b.Id).ToList()
                })
                .ToListAsync();
            return Result<List<PublisherReadDto>>.Success(result);


            
        }
    }
}
