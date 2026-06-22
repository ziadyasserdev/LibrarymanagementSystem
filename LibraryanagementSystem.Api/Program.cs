using Hangfire;
using LibrarymanagementSystem.Api.Extensions;
using LibrarymanagementSystem.Api.Filters;
using LibrarymanagementSystem.Api.Middleware;
using LibrarymanagementSystem.Application.BackgroundJobs.Fines;
using LibrarymanagementSystem.Application.BackgroundJobs.General;
using LibrarymanagementSystem.Application.Contracts.Identity;
using LibrarymanagementSystem.Application.Contracts.Services;
using LibrarymanagementSystem.Application.Extensions;
using LibrarymanagementSystem.Application.Settings;
using LibrarymanagementSystem.Data.Identity;
using LibrarymanagementSystem.Infrastructure.Extension;
using LibrarymanagementSystem.Infrastructure.Identity;
using LibrarymanagementSystem.Infrastructure.Persistence.Context;
using LibrarymanagementSystem.Infrastructure.Persistence.SeedData;
using LibrarymanagementSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;


namespace LibraryanagementSystem.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("DbConn")));

            builder.Services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();




            builder.Services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter(policyName: "Fixed", options =>
                {
                    options.PermitLimit = 5;
                    options.Window = TimeSpan.FromMinutes(1);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = 2;
                });
            });









            builder.Services.AddInfrastructureServices(builder.Configuration)
                .AddApplicationDependency()
              ;
            builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration
                .GetConnectionString("DbConn")));
            builder.Services.AddHangfireServer();

            builder.Services.Configure<JwtSetting>(
                builder.Configuration.GetSection("JwtSetting")
            );
            builder.Services.Configure<FineSettings>(
              builder.Configuration.GetSection("FineSettings")
          );
            builder.Services.Configure<EmailSettings>(
               builder.Configuration.GetSection("EmailSettings")
           );

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateIssuerSigningKey = true,

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["JwtSetting:Issuer"],
                    ValidAudience = builder.Configuration["JwtSetting:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.
                    Configuration["JwtSetting:Key"])),
                    ClockSkew=TimeSpan.Zero
                };
            });
            //          
            builder.Services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();

                options.OrderActionsBy(apiDesc =>
        apiDesc.ActionDescriptor.RouteValues["controller"]);
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Library Management API",
                    Version = "v1"
                });

                options.DocumentFilter<SortPathsDocumentFilter>();







                options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme,
    securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization : `Bearer Genreated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = JwtBearerDefaults.AuthenticationScheme
            }
        },
        new string[] { }
    }
});

            });

            builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters
            .Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });



            builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters
            .Add(new JsonStringEnumConverter());
    });


            builder.Services.AddControllers()
.AddJsonOptions(options =>
{
options.JsonSerializerOptions.Converters.Add(
     new JsonStringEnumConverter());
});
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.Configure<FileStorageSettings>(builder.Configuration.GetSection("FileStorageSettings"));
            builder.Services.Configure<LibraryPolicySetting>(builder.Configuration.GetSection("LibraryPolicySetting"));
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

                await RoleSeeder.SeedAsync(roleManager);
                await UserSeeder.SeedAsync(userManager);
                await CategorySeeder.SeedAsync(context);
                await AuthorSeeder.SeedAsync(context);
                await PublisherSeeder.SeedAsync(context);
                await BranchSeeder.SeedAsync(context);
                await LocationSeeder.SeedAsync(context);
                await BookSeeder.SeedAsync(context);

            }



            app.UseSwagger();
                app.UseSwaggerUI();
        
            app.UseStaticFiles();
            app.UseRateLimiter();
          app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();



            //using (var scope = app.Services.CreateScope())
            //{
            //    var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
            //    var fineJobs = scope.ServiceProvider.GetRequiredService<IFineJobs>();

            //    recurringJobManager.AddOrUpdate(
            //        "calculate-fines-job",
            //        () => fineJobs.CalculateFinesForOverdueBooks(),
            //        "*/10 * * * *"
            //    );
            //}



         app.UseHangfireDashboard("/dashboard");
            RecurringJob.AddOrUpdate<ReservationExpiryJob>(
    "expire-reservations-job",
    job => job.Execute(),
    "*/10 * * * *"
);

            app.MapControllers();
     
            app.Run();
        }
    }
}
