using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Authors.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorByName
{
    public class GetAuthorByNameQueryHandler : IRequestHandler<GetAuthorByNameQuery, Result<AuthorDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAuthorByNameQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<AuthorDto>> Handle(GetAuthorByNameQuery request, CancellationToken cancellationToken)
        {
            var author = await unitOfWork.Authors.GetAuthorByName(request.Name);
            if (author == null)
            {

                return Result<AuthorDto>.Failure(ResultStatus.NotFound, "Author not found");
            }
            var authorDto = new AuthorDto
            {
                AuthorId = author.AuthorId,
                Name = author.Name,
                Biography = author.Biography,
                DateOfBirth = author.DateOfBirth,
            };
            return Result<AuthorDto>.Success(authorDto);

        }
    }
}
