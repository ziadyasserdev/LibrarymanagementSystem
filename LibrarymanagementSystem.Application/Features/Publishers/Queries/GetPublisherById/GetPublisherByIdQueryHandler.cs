using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Publishers.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Queries.GetPublisherById
{
    /*
     public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

     Books=publisher.Books.select(c=>new 
        public List<BookDto> Books { get; set; } = new();
     */
    public class GetPublisherByIdQueryHandler : IRequestHandler<GetPublisherByIdQuery, Result<PublisherDetailDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetPublisherByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<PublisherDetailDto>> Handle(GetPublisherByIdQuery request, CancellationToken cancellationToken)
        {
            var publisher = await unitOfWork.Publishers.Query()
                .Include(c=>c.Books).FirstOrDefaultAsync(x=>x.Id == request.Id);
            if (publisher == null)
                return Result<PublisherDetailDto>.Failure(ResultStatus.NotFound, "Publisher not found");
            var publisherDto = new PublisherDetailDto
            {
                Id=publisher.Id,
                Name=publisher.Name,
                Email=publisher.Email,
                Phone=publisher.Phone,
                Website=publisher.Website,
                IsActive=publisher.IsActive,
                CreatedAt=publisher.CreatedAt,
                Books=publisher.Books.Select(c=> new BookDto
                {
                    Id = c.Id,
                    Title=c.Title,
                }).ToList(),
            };
            return Result<PublisherDetailDto>.Success(publisherDto);
        }
    }
}
