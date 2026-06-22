using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Books.Queries.GetBooksCount
{
    public class GetBooksCountQueryHandler : IRequestHandler<GetBooksCountQuery, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetBooksCountQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(GetBooksCountQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Books.Query();
            if (request.IncludeDeleted.HasValue)
                query = query.Where(x => x.IsDeleted == request.IncludeDeleted.Value);
            var count = await query.CountAsync();
            return Result<int>.Success(count);
        }
    }
}
