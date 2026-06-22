using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Features.Reviews.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Commands.CreateReview
{
   
    public class CreateReviewCommand:IRequest<Result<ReviewDto>>
    {
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public int BookId { get; set; }
    }
}
