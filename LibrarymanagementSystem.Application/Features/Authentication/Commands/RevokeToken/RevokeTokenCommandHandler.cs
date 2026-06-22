using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authentication.Commands.RevokeToken
{
    public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, Result<int>>
    {
        private readonly IAuthService authService;
        private readonly ICurrentUserService currentUserService;

        public RevokeTokenCommandHandler(IAuthService authService,ICurrentUserService currentUserService)
        {
            this.authService = authService;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<int>.Failure(ResultStatus.Unauthorized, "User is not authenticated");
            var result = await authService.RevokeTokenAsync(request.refreshToken);
            return Result<int>.Success(1, "Token revoked successfully");
        }
    }
}
