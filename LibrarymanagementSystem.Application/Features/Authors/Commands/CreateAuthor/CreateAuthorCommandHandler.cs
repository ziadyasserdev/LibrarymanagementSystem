using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Authors.Dtos;
using LibrarymanagementSystem.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Commands.CreateAuthor
{
    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, Result<AuthorDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public CreateAuthorCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<AuthorDto>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            var checkNameExist=await unitOfWork.Authors.Query()
                .AnyAsync(x=>x.Name== request.Name && !x.IsDeleted);

            if(checkNameExist)
                return Result<AuthorDto>.Failure(ResultStatus.Failure, "Author name already exist");
            var author = new Author
            {
                Name=request.Name,
                Biography=request.Biography,
                DateOfBirth=request.DateOfBirth,
                CreatedAt=DateTime.Now,
             
            };
            await unitOfWork.Authors.AddAsync(author);
            await unitOfWork.SaveAsync();
            var authorDto = new AuthorDto
            {
                AuthorId=author.AuthorId,
                Name=author.Name,
                Biography=author.Biography,
                DateOfBirth=author.DateOfBirth,
                CreatedAt=author.CreatedAt,
               

            };

            return Result<AuthorDto>.Success(authorDto);
        }
    }
}
