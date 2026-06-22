using LibrarymanagementSystem.Application.Common.PaginatedResults;
using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.FinePayments.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetAllFinePaymentsWithPagination
{
    public class GetAllFinePaymentsWithPaginationQuery:IRequest<Result<PaginatedResult<FinePaymentDto>>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public GetAllFinePaymentsWithPaginationQuery(int ps,int pn)
        {
            PageSize=ps;
            PageNumber=pn;
        }
    }
}
