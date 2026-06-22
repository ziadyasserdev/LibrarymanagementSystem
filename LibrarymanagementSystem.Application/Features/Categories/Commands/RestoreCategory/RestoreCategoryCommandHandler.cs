using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Commands.RestoreCategory
{
    public class RestoreCategoryCommandHandler : IRequestHandler<RestoreCategoryCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public RestoreCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(RestoreCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.Categories
             .Query()
             .IgnoreQueryFilters()
             .FirstOrDefaultAsync(x => x.CategoryId == request.Id);

            if (category is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Category not found");

            if (!category.IsDeleted)
                return Result<int>.Failure(ResultStatus.Failure,
                    "Category is not deleted to restore it");

            var checkDuplicated = await unitOfWork.Categories
                .Query()
                .AnyAsync(x => x.Name == category.Name
                               && !x.IsDeleted
                               && x.CategoryId != category.CategoryId);

            if (checkDuplicated)
                return Result<int>.Failure(ResultStatus.Failure,
                    "Another active category with the same name already exists. Please rename the category before restoring it");

            category.IsDeleted = false;
            category.DeletedAt = null;
            category.UpdatedAt = DateTime.UtcNow;

            await unitOfWork.SaveAsync();

            return Result<int>.Success(category.CategoryId);

        }
    }
}
