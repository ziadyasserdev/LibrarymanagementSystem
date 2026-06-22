using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.FinePayments.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetRevenuePayments
{
    public class GetRevenuePaymentsQuery:IRequest<Result<RevenueResponseDto>>
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public GetRevenuePaymentsQuery(DateTime f , DateTime t)
        {
            To = t;
            From = f;
        }
    }
}
