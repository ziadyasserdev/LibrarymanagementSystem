using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Authors.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.GetAllAuthors
{
    public class GetAllAuthorQueryHandler : IRequestHandler<GetAllAuthorQuery, Result<List<AuthorDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllAuthorQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<AuthorDto>>> Handle(GetAllAuthorQuery request, CancellationToken cancellationToken)
        {
            var authors = await unitOfWork.Authors
                .GetAllAsync();
            if(authors==null|| !authors.Any())
                return Result<List<AuthorDto>>.Failure(ResultStatus.NotFound, "No authors found");
            var authorsDto = authors.Select(x => new AuthorDto
            {
                AuthorId = x.AuthorId,
                Name = x.Name,
                Biography = x.Biography,
                DateOfBirth=x.DateOfBirth

            }).ToList();
             return Result<List<AuthorDto>>.Success(authorsDto);

        }
    }
}
