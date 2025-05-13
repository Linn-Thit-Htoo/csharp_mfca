using csharp_mfca.API.Configurations;
using csharp_mfca.API.Entities;
using csharp_mfca.API.Exceptions;
using csharp_mfca.API.Features.Users.CreateUser;
using csharp_mfca.API.Middlewares;
using csharp_mfca.API.Persistence.Wrapper;
using csharp_mfca.API.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace csharp_mfca.API.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(opt =>
            {
                opt.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context
                        .ModelState.Where(ms => ms.Value!.Errors.Any())
                        .SelectMany(ms => ms.Value!.Errors.Select(e => e.ErrorMessage))
                        .ToList();

                    var errorMessage = string.Join(". ", errors);
                    var result = BaseResponse<object>.Fail(context.HttpContext.TraceIdentifier, errorMessage);

                    return new OkObjectResult(result);
                };
            })
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.PropertyNamingPolicy = null;
                opt.JsonSerializerOptions.DictionaryKeyPolicy = null;
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHealthChecks();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IPasswordHasher<TblUser>, PasswordHasher<TblUser>>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.Configure<AppSetting>(builder.Configuration);
        builder.Services.AddBusinessLogicServices();
        builder.Services.AddDataAccessServices();
        builder.Services.AddValidatorsFromAssembly(typeof(DependencyInjectionExtensions).Assembly);
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
            opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                "CORSPolicy",
                builder =>
                    builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetIsOriginAllowed((hosts) => true)
            );
        });

        return services;
    }

    public static IApplicationBuilder UseTraceMiddleware(this WebApplication app)
    {
        return app.UseMiddleware<TraceIdMiddleware>();
    }

    private static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
    {
        return services.AddScoped<BL_CreateUser>();
    }

    private static IServiceCollection AddDataAccessServices(this IServiceCollection services)
    {
        return services.AddScoped<DA_CreateUser>();
    }
}
