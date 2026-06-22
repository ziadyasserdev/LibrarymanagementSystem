using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Commands.PayFine
{
 
    public class PayFineCommand:IRequest<Result<string>>
    {
        public int FineId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PayStatus PayStatus { get; set; }

    }
}
