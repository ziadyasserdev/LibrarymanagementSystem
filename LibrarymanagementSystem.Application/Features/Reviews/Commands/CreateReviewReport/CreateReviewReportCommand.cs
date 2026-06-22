using LibrarymanagementSystem.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Commands.CreateReviewReport
{
    public class CreateReviewReportCommand:IRequest<Result<int>>
    {
        public int ReviewId { get; set; }
        public string Reason { get; set; }

        public CreateReviewReportCommand(int reviewId, string reason)
        {
            ReviewId = reviewId;
            Reason = reason;
        }
    }
}
