using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.Categories.Query()
               
                .FirstOrDefaultAsync(x => x.CategoryId == request.Id && !x.IsDeleted);
            if (category == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Category not found.");
            var hasBooks = await unitOfWork.Books.Query()
                   .AnyAsync(x => x.CategoryId == request.Id);
            if(hasBooks)
                return Result<int>.Failure(ResultStatus.Failure, "Cannot delete category with associated books.");  
            category.IsDeleted = true;
            category.DeletedAt = DateTime.Now;
           
            await unitOfWork.SaveAsync();
            return Result<int>.Success(category.CategoryId);
        }
    }
}
