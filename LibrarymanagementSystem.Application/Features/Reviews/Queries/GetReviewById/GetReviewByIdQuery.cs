using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Reviews.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.GetReviewById
{
    public class GetReviewByIdQuery:IRequest<Result<AdminReviewDto>>
    {
        public int Id { get; set; }
        public GetReviewByIdQuery(int id)
        {
            this.Id = id;
        }
    }
}
