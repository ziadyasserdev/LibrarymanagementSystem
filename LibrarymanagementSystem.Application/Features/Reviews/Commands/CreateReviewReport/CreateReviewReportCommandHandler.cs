using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Reviews.Dtos;
using LibrarymanagementSystem.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Commands.CreateReviewReport
{
    public class CreateReviewReportCommandHandler : IRequestHandler<CreateReviewReportCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public CreateReviewReportCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(CreateReviewReportCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<int>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated."
                );

            var userId = currentUserService.UserId!;
            var review = await unitOfWork.Reviews.Query()
                .FirstOrDefaultAsync(r => r.Id == request.ReviewId);
            if (review is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Review not found.");

            var report = new ReviewReport
            {
                ReviewId = request.ReviewId,
                UserId = userId,
                Reason = request.Reason
            };

            await unitOfWork.ReviewReports.AddAsync(report);
            await unitOfWork.SaveAsync();

            return Result<int>.Success(report.Id);
        }
    }
}
