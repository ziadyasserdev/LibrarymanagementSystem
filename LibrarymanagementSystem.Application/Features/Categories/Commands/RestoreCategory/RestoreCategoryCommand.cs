using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Commands.RestoreCategory
{
    public class RestoreCategoryCommand:IRequest<Result<int>>
    {
        public int Id { get; set; }
         
    }
}
