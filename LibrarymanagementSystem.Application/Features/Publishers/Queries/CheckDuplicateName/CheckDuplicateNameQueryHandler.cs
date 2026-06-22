using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Publishers.Queries.CheckDuplicateName
{
    public class CheckDuplicateNameQueryHandler : IRequestHandler<CheckDuplicateNameQuery, Result<bool>>
    {
        private readonly IUnitOfWork unitOfWork;

        public CheckDuplicateNameQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<bool>> Handle(CheckDuplicateNameQuery request, CancellationToken cancellationToken)
        {
           var check=await unitOfWork.Publishers.Query()
                .AnyAsync(x => x.Name == request.Name && !x.IsDeleted);
                return Result<bool>.Success(check);
        }
    }
}
