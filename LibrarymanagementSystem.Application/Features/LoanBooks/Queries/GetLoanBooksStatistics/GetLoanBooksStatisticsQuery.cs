using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.LoanBooks.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetLoanBooksStatistics
{
    public class GetLoanBooksStatisticsQuery:IRequest<Result<LibraryDashboardDto>>
    {
    }
}
