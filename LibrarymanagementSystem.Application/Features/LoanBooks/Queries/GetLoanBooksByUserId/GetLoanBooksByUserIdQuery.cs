using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.LoanBooks.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetLoanBooksByUserId
{
    public class GetLoanBooksByUserIdQuery:IRequest<Result<List<LoanBookDetailForAdminDto>>>
    {
        public string UserId { get; set; }
        public GetLoanBooksByUserIdQuery(string id)
        {
            UserId=id;
        }
    }
}
