using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Commands.BulkSoftDeleteCategory
{
    public class BulkSoftDeleteCategoryCommand:IRequest<Result<string>>
    {
        public List<int> CategoryIds { get; set; }
        public BulkSoftDeleteCategoryCommand(List<int> CategoryIds)
        {
            this.CategoryIds = CategoryIds;
        }
    }
}
