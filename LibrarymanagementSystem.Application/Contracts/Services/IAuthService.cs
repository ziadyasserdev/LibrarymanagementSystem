using LibrarymanagementSystem.Application.Features.Authentication.Dtos;
using LibrarymanagementSystem.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Contracts.Services
{
    public interface IAuthService
    {
        public Task<AuthTokenDto> GenerateToken(ApplicationUser user);
        public RefreshToken GenerateRefreshToken();
        public Task<AuthTokenDto> RefreshTokenAsync(string token);
        public Task<bool> RevokeTokenAsync(string token);
    }
}
