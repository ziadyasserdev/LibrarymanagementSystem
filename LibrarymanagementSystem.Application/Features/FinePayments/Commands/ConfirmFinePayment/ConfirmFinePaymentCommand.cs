using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.FinePayments.Commands.ConfirmFinePayment
{
    public class ConfirmFinePaymentCommand:IRequest<Result<int>>
    {
        public int FinePaymentId { get; set; }
        public bool Success { get; set; }
    }
}
