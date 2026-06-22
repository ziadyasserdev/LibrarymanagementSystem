using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.Categories.Query()
                .FirstOrDefaultAsync(x => x.CategoryId == request.Id && !x.IsDeleted);
            if (category==null)
                return Result<int>.Failure(ResultStatus.NotFound, "Category not found");
            category.Name = request.Name;
            category.Description = request.Description;
            category.UpdatedAt = DateTime.Now;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(category.CategoryId);
        }
    }
}
