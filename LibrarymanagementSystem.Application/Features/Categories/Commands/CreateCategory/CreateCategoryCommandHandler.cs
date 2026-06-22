using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Categories.Dtos;
using LibrarymanagementSystem.Data.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<CategoryDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category=new Category
            {
                Name=request.Name,
                Description=request.Description,
                CreatedAt=DateTime.Now
               

            };
            await unitOfWork.Categories.AddAsync(category);
            await unitOfWork.SaveAsync();
            var categoryDto=new CategoryDto
            {
                CategoryId=category.CategoryId,
                Name=category.Name,
                Description=category.Description
            };
            return Result<CategoryDto>.Success(categoryDto);
        }
    }
}
