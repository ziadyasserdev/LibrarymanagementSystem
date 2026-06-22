using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authors.Queries.CheckAuthorExistsByName
{
    public class CheckAuthorExistsByNameQueryHandler : IRequestHandler<CheckAuthorExistsByNameQuery, Result<bool>>
    {
        private readonly IUnitOfWork unitOfWork;

        public CheckAuthorExistsByNameQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<bool>> Handle(CheckAuthorExistsByNameQuery request, CancellationToken cancellationToken)
        {
            var exists =await unitOfWork.Authors.Query()
                .AnyAsync(x => x.Name == request.Name && !x.IsDeleted);
            return Result<bool>.Success(exists);
        }
    }
}
