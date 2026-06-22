using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Categories.Queries.GetCategoryCount
{
    public class GetCategoryCountQueryHandler : IRequestHandler<GetCategoryCountQuery, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetCategoryCountQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(GetCategoryCountQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Categories.Query();

            if (request.IsDeleted.HasValue)
                query = query.Where(x => x.IsDeleted == request.IsDeleted.Value);

            var count = await query.CountAsync(cancellationToken);

            return Result<string>.Success(count.ToString());
        }
    }
}
