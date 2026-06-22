using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Reviews.Queries.GetRatingDistribution
{
    public class GetRatingDistributionQueryHandler : IRequestHandler<GetRatingDistributionQuery, Result<Dictionary<int, int>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetRatingDistributionQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<Dictionary<int, int>>> Handle(GetRatingDistributionQuery request, CancellationToken cancellationToken)
        {
            var exist=await unitOfWork.Books.Query()
                .AnyAsync(x => x.Id == request.Id);
            if (!exist)
                return Result<Dictionary<int, int>>.Failure(ResultStatus.NotFound,"Book not found.");
            var ratings = await unitOfWork.Reviews.Query()
                 .Where(r => r.BookId == request.Id)
                 .GroupBy(r => r.Rating)
                 .Select(g => new { Rating = g.Key, Count = g.Count() })
                 .ToListAsync();
            var distribution = new Dictionary<int, int>
            {
                {1, 0},
                {2, 0},
                {3, 0},
                {4, 0},
                {5, 0}
            };

            foreach (var r in ratings)
            {
                distribution[r.Rating] = r.Count;
            }
            return Result<Dictionary<int, int>>.Success(distribution);
        }
    }
}
