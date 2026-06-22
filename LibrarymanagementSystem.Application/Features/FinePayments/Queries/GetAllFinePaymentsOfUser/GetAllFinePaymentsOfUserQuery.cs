using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.FinePayments.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetAllFinePaymentsOfUser
{
    public class GetAllFinePaymentsOfUserQuery:IRequest<Result<List<FinePaymentDto>>>
    {
        public string userId { get; set; }
        public GetAllFinePaymentsOfUserQuery(string id)
        {
            this.userId = id;   
        }
    }
}
