using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Data.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Fines.Commands.AddFine
{
    public class AddFineCommand:IRequest<Result<int>>
    {
        public int LoanBookId { get; set; }

        public decimal TotalAmount { get; set; }

        public FineType FineType { get; set; }

        public string? Notes { get; set; }
    }
}
