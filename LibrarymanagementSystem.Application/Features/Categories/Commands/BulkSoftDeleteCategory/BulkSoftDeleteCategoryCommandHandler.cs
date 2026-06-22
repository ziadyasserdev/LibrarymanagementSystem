using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Commands.BulkSoftDeleteCategory
{
    public class BulkSoftDeleteCategoryCommandHandler : IRequestHandler<BulkSoftDeleteCategoryCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public BulkSoftDeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(BulkSoftDeleteCategoryCommand request, CancellationToken cancellationToken)
        {

            var distinctIds = request.CategoryIds.Distinct().ToList();

           
            var categoriesToDelete = await unitOfWork.Categories.Query()
                .Where(c => distinctIds.Contains(c.CategoryId) && !c.IsDeleted)
                .ToListAsync(cancellationToken);

            if (!categoriesToDelete.Any())
                return Result<string>.Failure(ResultStatus.NotFound,"No categories found to delete.");

          
            var categoriesWithBooks = await unitOfWork.Books.Query()
                .Where(b => distinctIds.Contains(b.CategoryId))
                .Select(b => b.CategoryId)
                .Distinct()
                .ToListAsync(cancellationToken);

           
            foreach (var category in categoriesToDelete)
            {
                if (categoriesWithBooks.Contains(category.CategoryId))
                    return Result<string>.Failure(ResultStatus.Failure,$"Cannot delete category '{category.Name}' because it has associated books.");

                category.IsDeleted = true;
                category.DeletedAt = DateTime.Now;
                category.UpdatedAt = DateTime.Now;
            }
            await unitOfWork.SaveAsync();
            return Result<string>.Success($"{categoriesToDelete.Count} Categories deleted successfully");
        }
    }
}
