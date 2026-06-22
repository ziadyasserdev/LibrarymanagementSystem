using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Commands.BulkRestoreCategory
{
    public class BulkRestoreCategoryCommand:IRequest<Result<string>>
    {
        public List<int> CategoryIds { get; set; }
        public BulkRestoreCategoryCommand(List<int> CategoryIds)
        {
            this.CategoryIds=CategoryIds;
        }
    }
}
