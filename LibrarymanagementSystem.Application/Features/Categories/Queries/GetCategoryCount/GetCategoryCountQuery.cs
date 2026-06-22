using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Queries.GetCategoryCount
{
    public class GetCategoryCountQuery:IRequest<Result<string>>
    {
        public bool? IsDeleted { get; set; }
    }
}
