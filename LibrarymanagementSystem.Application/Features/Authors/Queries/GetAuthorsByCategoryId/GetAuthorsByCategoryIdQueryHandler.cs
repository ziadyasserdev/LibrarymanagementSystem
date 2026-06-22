using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Authors.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorsByCategoryId
{
    public class GetAuthorsByCategoryIdQueryHandler : IRequestHandler<GetAuthorsByCategoryIdQuery, Result<List<AuthorDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAuthorsByCategoryIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<AuthorDto>>> Handle(GetAuthorsByCategoryIdQuery request, CancellationToken cancellationToken)
        {

            var category = await unitOfWork.Categories.GetByIdAsync(request.CategoryId);
            if (category==null)
          
                return Result<List<AuthorDto>>.Failure(ResultStatus.NotFound, "Category not found");

            var authors = await unitOfWork.Books
       .Query()
       .Where(b => b.CategoryId == request.CategoryId)
       .Select(b => b.Author)
       .Distinct()
       .Select(a => new AuthorDto
       {
           AuthorId = a.AuthorId,
           Name = a.Name,
           Biography = a.Biography,
           DateOfBirth = a.DateOfBirth
       })
       .ToListAsync();

      



            if (!authors.Any())
                return Result<List<AuthorDto>>
                    .Failure(ResultStatus.NotFound, "No authors found for this category");

            return Result<List<AuthorDto>>.Success(authors);


        }
    }
}
