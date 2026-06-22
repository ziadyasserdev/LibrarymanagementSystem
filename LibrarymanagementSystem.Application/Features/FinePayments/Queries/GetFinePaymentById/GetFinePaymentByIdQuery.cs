using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.FinePayments.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Queries.GetFinePaymentById
{
    public class GetFinePaymentByIdQuery:IRequest<Result<FinePaymentDto>>
    {
        public int Id { get; set; }
        public GetFinePaymentByIdQuery(int id)
        {
            this.Id = id;   
        }
    }
}
