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

namespace LibrarymanagementSystem.Application.Features.Publishers.Queries.GetAllPublishers
{
    public class GetAllPublishersQueryHandler : IRequestHandler<GetAllPublishersQuery, Result<List<PublisherReadDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllPublishersQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<PublisherReadDto>>> Handle(GetAllPublishersQuery request, CancellationToken cancellationToken)
        {
            var publishers = await unitOfWork.Publishers.Query()
                .Include(c => c.Books)
                .ToListAsync();
       
               
            if (!publishers.Any())
                return Result<List<PublisherReadDto>>.Failure(ResultStatus.NotFound, "Publishers not found");
            var publishersDto=publishers.Select(x => new PublisherReadDto
            {
                Id=x.Id,
                Name=x.Name,
                Email=x.Email,
                Phone=x.Phone,
                Website=x.Website,
                IsActive=!x.IsDeleted,
                BookIds=x.Books.Select(x=>x.Id).ToList(),
            }).ToList();

            return Result<List<PublisherReadDto>>.Success(publishersDto);
        }
    }
}
