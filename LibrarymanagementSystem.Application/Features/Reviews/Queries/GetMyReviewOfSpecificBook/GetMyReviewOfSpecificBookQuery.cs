using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Reviews.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.GetMyReviewOfSpecificBook
{
    public class GetMyReviewOfSpecificBookQuery:IRequest<Result<MyReviewDto>>
    {
        public int BookId { get; set; }
        public GetMyReviewOfSpecificBookQuery(int BookId)
        {
            this.BookId = BookId;
        }
    }
}
