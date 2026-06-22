using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.GetAuthorCount
{
    public class GetAuthorCountQueryHandler : IRequestHandler<GetAuthorCountQuery, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAuthorCountQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(GetAuthorCountQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Authors.Query();
            if(request.IsDeleted.HasValue)
                query = query.Where(x => x.IsDeleted == request.IsDeleted.Value);
            return Result<int>.Success(await query.CountAsync(cancellationToken));
        }
    }
}
