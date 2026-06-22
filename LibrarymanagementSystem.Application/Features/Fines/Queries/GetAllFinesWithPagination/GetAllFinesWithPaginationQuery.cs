using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Fines.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetAllFinesWithPagination
{
    public class GetAllFinesWithPaginationQuery:IRequest<Result<PaginatedResult<FineDto>>>
    {
        public int PageNumber { get; set; } 
        public int PageSize { get; set; }
        public GetAllFinesWithPaginationQuery(int pn,int ps)
        {
            this.PageNumber=pn;
            this.PageSize=ps;
        }
    }
}
