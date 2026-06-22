using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.Services;
using LibrarymanagementSystem.Application.Features.Authentication.Dtos;
using LibrarymanagementSystem.Application.Settings;
using LibrarymanagementSystem.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly JwtSetting jwtSetting;
        private readonly RoleManager<ApplicationRole> roleManager;
        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JwtSetting> options, RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.jwtSetting = options.Value;
            this.roleManager = roleManager;
        }
        public async Task<AuthTokenDto> GenerateToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(JwtRegisteredClaimNames.Email,user.Email??""),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName??""),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName ?? ""),


            };

            var userCliams = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
                var roleName = await roleManager.FindByNameAsync(role);
                var roleCliams = await roleManager.GetClaimsAsync(roleName!);
                foreach (var claim in roleCliams)
                {
                    userCliams.Add(claim);
                }

            }
            claims.AddRange(userCliams);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtSetting.Issuer,
                audience: jwtSetting.Audience,
                claims: claims,
                signingCredentials: cred,
                expires: DateTime.UtcNow.AddMinutes(jwtSetting.DurationInMinutes)
                );
            var authToken = new AuthTokenDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                IsAuthenticated = true,
                Message="Process Success"
                //Duration = jwtSetting.DurationInMinutes
            };
            return authToken;
        }
        public RefreshToken GenerateRefreshToken()
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            return new RefreshToken
            {
                Token = token,
                CreatedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(7)
            };
        }

        public async Task<AuthTokenDto> RefreshTokenAsync(string token)
        {
            var result = new AuthTokenDto();
            var user = await userManager.Users.SingleOrDefaultAsync(x => x.RefreshTokens.Any(i => i.Token == token));
            if (user == null)
            {
                result.IsAuthenticated = false;
                result.Message = "Invalid Token";
                return result;
            }

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
            {
                result.IsAuthenticated = false;
                result.Message = "Inactive token";
                return result;
            }

            refreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await userManager.UpdateAsync(user);

            result= await GenerateToken(user);
            result.IsAuthenticated = true;
            
           
        
            result.RefreshToken = newRefreshToken.Token;
            result.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

            return result;

        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await userManager.Users.SingleOrDefaultAsync(x => x.RefreshTokens.Any(i => i.Token == token));
            if (user == null)
                return false;
            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                return false;
            refreshToken.RevokedOn = DateTime.UtcNow;
            await userManager.UpdateAsync(user);
            return true;
        }
    }
}
