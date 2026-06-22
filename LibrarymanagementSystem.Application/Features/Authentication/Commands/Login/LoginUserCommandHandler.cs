using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.ExternalServices;
using LibrarymanagementSystem.Application.Contracts.Services;
using LibrarymanagementSystem.Application.Features.Authentication.Dtos;
using LibrarymanagementSystem.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authentication.Commands.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<AuthTokenDto>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAuthService authService;
        private readonly IEmailService emailService;

        public LoginUserCommandHandler(UserManager<ApplicationUser> userManager,IAuthService authService,IEmailService emailService)
        {
            this.userManager = userManager;
            this.authService = authService;
            this.emailService = emailService;
        }
        public async Task<Result<AuthTokenDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var result = new AuthTokenDto();

            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return Result<AuthTokenDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Invalid email or password.");

            if (user.IsDeleted)
                return Result<AuthTokenDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Invalid email or password.");
            if (user.IsLocked)
                return Result<AuthTokenDto>.Failure(
                    ResultStatus.Forbidden,
                    "Your account is locked. Contact admin.");

            var passwordValid = await userManager.CheckPasswordAsync(user, request.Password);

            if (!passwordValid)
                return Result<AuthTokenDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Invalid email or password.");

            // لو حابب تفعّلها
            // if (!user.EmailConfirmed)
            //     return Result<AuthTokenDto>.Failure(
            //         ResultStatus.Forbidden,
            //         "Please confirm your email first.");

            if (user.TwoFactorEnabled)
            {
                var code = await userManager.GenerateTwoFactorTokenAsync(
                    user,
                    TokenOptions.DefaultEmailProvider);

                await emailService.SendEmailAsync(
                    user.Email!,
                    "Verification Code",
                    $"Your verification code is {code}");

                return Result<AuthTokenDto>.Failure(
     ResultStatus.RequiresTwoFactor,
     "Two factor authentication required");
            }

            result = await authService.GenerateToken(user);

            if (user.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.First(t => t.IsActive);

                result.RefreshToken = activeRefreshToken.Token;
                result.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = authService.GenerateRefreshToken();

                result.RefreshToken = refreshToken.Token;
                result.RefreshTokenExpiration = refreshToken.ExpiresOn;

                user.RefreshTokens.Add(refreshToken);

                await userManager.UpdateAsync(user);
            }

            return Result<AuthTokenDto>.Success(result, "User logged in successfully.");
        }
    }
}
