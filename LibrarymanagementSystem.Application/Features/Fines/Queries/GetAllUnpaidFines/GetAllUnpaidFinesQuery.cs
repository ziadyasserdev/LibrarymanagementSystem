using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Fines.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Queries.GetAllUnpaidFines
{
    public class GetAllUnpaidFinesQuery:IRequest<Result<PaginatedResult<FineDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public GetAllUnpaidFinesQuery(int pN, int pZ)
        {
            PageNumber = pN;
            PageSize = pZ;
        }
    }
}
