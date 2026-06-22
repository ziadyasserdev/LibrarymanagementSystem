using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authentication.Commands.LockUser
{
    public class LockUserCommandHandler : IRequestHandler<LockUserCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICurrentUserService currentUserService;

        public LockUserCommandHandler(UserManager<ApplicationUser> userManager,ICurrentUserService currentUserService)
        {
            this.userManager = userManager;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(LockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return Result<string>.Failure(ResultStatus.NotFound, "User not found");
            if(user.IsLocked)
                return Result<string>.Failure(ResultStatus.Conflict, "User is already locked");
            user.IsLocked = true;
            user.LockedAt=DateTime.UtcNow;
            user.LockedByAdminId=currentUserService.UserId;
            await userManager.UpdateAsync(user);
            return Result<string>.Success(user.Id, "User locked successfully");
        }
    }
}
