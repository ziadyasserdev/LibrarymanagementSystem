using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.FinePayments.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetFinePaymentSummary
{
    public class GetFinePaymentSummaryQuery:IRequest<Result<FinePaymentSummaryDto>>
    {
        public int FineId { get; set; }
        public GetFinePaymentSummaryQuery(int id)
        {
            FineId = id;
        }
    }
}
