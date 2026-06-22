
using LibrarymanagementSystem.Application.Contracts.Repositories;
using LibrarymanagementSystem.Application.Settings;
using LibrarymanagementSystem.Data.Identity;
using LibrarymanagementSystem.Infrastructure.Persistence.Context;
using LibrarymanagementSystem.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using LibrarymanagementSystem.Application.Contracts.Services;
using LibrarymanagementSystem.Infrastructure.Services;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Infrastructure.Identity;
using LibrarymanagementSystem.Application.Contracts.ExternalServices;
using LibrarymanagementSystem.Infrastructure.ExternalService;


namespace LibrarymanagementSystem.Infrastructure.Extension
{
    public static class InfrastructureDependencyRegisteration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
          
            services.AddScoped<IUnitOfWork, UnitOfWorkRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICacheService, MemoryCacheService>();
            services.AddMemoryCache();
            return services;

        }
    }
}
