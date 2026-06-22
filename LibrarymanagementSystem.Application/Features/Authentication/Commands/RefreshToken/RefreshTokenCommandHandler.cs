using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Services;
using LibrarymanagementSystem.Application.Features.Authentication.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authentication.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthTokenDto>>
    {
        private readonly IAuthService authService;
        private readonly ICurrentUserService currentUserService;

        public RefreshTokenCommandHandler(IAuthService authService,ICurrentUserService currentUserService)
        {
            this.authService = authService;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<AuthTokenDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
           
            var result =await authService.RefreshTokenAsync(request.refreshToken);
            if(!result.IsAuthenticated)
                return Result<AuthTokenDto>.Failure(ResultStatus.Failure, result.Message!);

            return Result<AuthTokenDto>.Success(result);
        }
    }
}
