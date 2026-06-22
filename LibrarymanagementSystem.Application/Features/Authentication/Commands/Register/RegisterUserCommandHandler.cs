using LibrarymanagementSystem.Application.Common.Results;
using LibrarymanagementSystem.Application.Contracts.ExternalServices;
using LibrarymanagementSystem.Application.Contracts.Services;
using LibrarymanagementSystem.Application.Features.ApplicationUsers.Dtos;
using LibrarymanagementSystem.Application.Features.Authentication.Dtos;
using LibrarymanagementSystem.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Features.Authentication.Commands.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<AuthResponseDto>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAuthService authService;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;

        public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager
            ,IAuthService authService
            ,RoleManager<ApplicationRole> roleManager 
            , IEmailService emailService
            ,IConfiguration configuration)
        {
            this.userManager = userManager;
            this.authService = authService;
            this.roleManager = roleManager;
            this.emailService = emailService;
            this.configuration = configuration;
        }
        public async Task<Result<AuthResponseDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if(await userManager.FindByEmailAsync(request.Email) != null)
            {
                return Result<AuthResponseDto>.Failure(ResultStatus.Failure,"Email already exists.");
            }
            var user=new ApplicationUser
            {
                UserName=request.Email,
                Email=request.Email,
                FullName=request.FullName,
                MembershipDate=DateTime.UtcNow,
                PhoneNumber=request.PhoneNumber,
                Gender=request.Gender
            };
            var result=await userManager.CreateAsync(user,request.Password);
            if(!result.Succeeded)
                {
                var errors=string.Join(", ",result.Errors.Select(e=>e.Description));
                return Result<AuthResponseDto>.Failure(ResultStatus.Failure,errors);
            }
            var role=await roleManager.FindByNameAsync("User".ToLower());
            if(role==null)
                return Result<AuthResponseDto>.Failure(ResultStatus.NotFound,"This role not found");
            if(!role.IsActive)
                return Result<AuthResponseDto>.Failure(ResultStatus.Failure, "This role is not active");
            var addToRoleResult= await userManager.AddToRoleAsync(user,"User");
            if (!addToRoleResult.Succeeded)
            {
                var errors = string.Join(" | ", addToRoleResult.Errors.Select(e => e.Description));

                
                return Result<AuthResponseDto>.Failure(
                    ResultStatus.Failure,
                    $"Failed to assign role. Reasons: {errors}"
                );
            }

            var token=await userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(
         Encoding.UTF8.GetBytes(token));

            var confirmationLink =
            $"{configuration["AppUrl"]}/api/Authentications/confirm-email?userId={user.Id}&token={encodedToken}";

            var body = $@"
            <h3>Confirm your email</h3>
            <p>Please click the link below:</p>
            <a href='{confirmationLink}'>Confirm Email</a>";

            await emailService.SendEmailAsync(user.Email, "Confirm Email", body);

            var Result =await authService.GenerateToken(user);
            var res = new UserResponseDto
            {
                Email=request.Email,
                Id=user.Id,
               Name=request.FullName,
               Role= "User"

            };
            var finalRes=new AuthResponseDto
            {
                Auth=Result,
                User=res,
            };
            return Result<AuthResponseDto>.Success(finalRes, "Registration successful.Please check your email.");
        }
    }
}
