using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.LoanBooks.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.LoanBooks.Queries.GetAllLateBooksByUserId
{
    public class GetAllLateBooksByUserIdQuery:IRequest<Result<List<LateBookDto>>>
    {
        public string UserId { get; set; }
        public GetAllLateBooksByUserIdQuery(string id)
        {
            UserId=id;
        }
    }
}
