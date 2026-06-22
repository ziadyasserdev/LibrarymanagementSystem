using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Authors.Dtos;
using LibrarymanagementSystem.Application.Features.Books.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorsWithPagination
{
    public class GetAuthorsWithPaginationQueryHandler : IRequestHandler<GetAuthorsWithPaginationQuery, Result<PaginatedResult<AuthorDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAuthorsWithPaginationQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<PaginatedResult<AuthorDto>>> Handle(GetAuthorsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Authors.Query()
                .AsNoTracking();
            var totalAuthors=await query.CountAsync();
            var items=await query.OrderBy(x=>x.AuthorId)
                .Skip((request.PageNumber-1)*request.PageSize)
                .Take(request.PageSize)
                .Select(x=>new AuthorDto
                {
                    AuthorId=x.AuthorId,
                    Name=x.Name,
                    Biography=x.Biography,
                    DateOfBirth=x.DateOfBirth,
                }).ToListAsync();

            var paginatedResult = new PaginatedResult<AuthorDto>(
             items,
             request.PageNumber,   
             request.PageSize,    
             totalAuthors         
         );


            return Result<PaginatedResult<AuthorDto>>.Success(paginatedResult);

        }
    }
}
