using LibrarymanagementSystem.Application.Common.PaginatedResults;
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

namespace LibrarymanagementSystem.Application.Features.Publishers.Queries.GetAllPublisherWithPagination
{
    public class GetAllPublisherWithPaginationQueryHandler : IRequestHandler<GetAllPublisherWithPaginationQuery, Result<PaginatedResult<PublisherReadDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllPublisherWithPaginationQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
      

        public async Task<Result<PaginatedResult<PublisherReadDto>>> Handle(GetAllPublisherWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query =  unitOfWork.Publishers.Query()
                .Include(c => c.Books).AsNoTracking();
           
        
            switch(request.Status)
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
            var totalCount = await query.CountAsync();
            var items =await query.OrderBy(c => c.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new PublisherReadDto
                {
                    Id = x.Id,
                    Name=x.Name,
                    Email=x.Email,
                    Phone=x.Phone,
                    Website=x.Website,
                    IsActive=x.IsActive,
                    CreatedAt=x.CreatedAt,
                    BookIds=x.Books.Select(x => x.Id).ToList(),
                }).ToListAsync();
            var paginatedResult=new PaginatedResult<PublisherReadDto>(items,request.PageNumber,request.PageSize,totalCount);
            return Result<PaginatedResult<PublisherReadDto>>.Success(paginatedResult);
        }
    }
}
