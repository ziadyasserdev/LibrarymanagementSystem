using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Commands.BulkRestoreCategory
{
    public class BulkRestoreCategoryCommandHandler : IRequestHandler<BulkRestoreCategoryCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public BulkRestoreCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(BulkRestoreCategoryCommand request, CancellationToken cancellationToken)
        {
            var distinctIds = request.CategoryIds.Distinct().ToList();

           
            var categoriesToRestore = await unitOfWork.Categories.Query()
                .Where(c => distinctIds.Contains(c.CategoryId) && c.IsDeleted)
                .ToListAsync(cancellationToken);

            if (!categoriesToRestore.Any())
                return Result<string>.Failure(ResultStatus.NotFound,"No deleted categories found to restore.");

          
            foreach (var category in categoriesToRestore)
            {
                category.IsDeleted = false;
                category.UpdatedAt = DateTime.Now;
                category.DeletedAt = null;
            }

          
            await unitOfWork.SaveAsync();
            return Result<string>.Success($"{categoriesToRestore.Count} Categories restored successfully");
        }
    }
}
