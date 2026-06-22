using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Features.Branches.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Branches.Queries.GetAllBranchesForUsers
{
    public class GetAllBranchesForUsersQueryHandler : IRequestHandler<GetAllBranchesForUsersQuery, Result<List<BranchUserDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllBranchesForUsersQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<BranchUserDto>>> Handle(GetAllBranchesForUsersQuery request, CancellationToken cancellationToken)
        {
            var branches = await unitOfWork.Branches.Query()
                .Where(x => !x.IsDeleted)
                .Select(x => new BranchUserDto
                {
                    Id = x.Id,
                    Name = x.Name,
                   Address=x.Address,
                   City = x.City,
                }).ToListAsync();
            return Result<List<BranchUserDto>>.Success(branches);
        }
    }
}
