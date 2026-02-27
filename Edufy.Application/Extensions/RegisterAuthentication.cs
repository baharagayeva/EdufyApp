using System.Text;
using Edufy.Domain.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Edufy.Domain.DTOs.AuthDTOs;

namespace Edufy.Application.Extensions;

public static class RegisterAuthentication
{
    public static void AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        var jwt = configuration.GetSection("Jwt").Get<JwtOptions>()
                  ?? throw new InvalidOperationException("Jwt settings are missing.");

        if (string.IsNullOrWhiteSpace(jwt.Key))
            throw new InvalidOperationException("Jwt:Key is missing.");

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // debugging üçün faydalıdır (dev-də saxla)
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = ctx =>
                    {
                        var logger = ctx.HttpContext.RequestServices
                            .GetRequiredService<ILoggerFactory>()
                            .CreateLogger("JwtAuth");

                        logger.LogWarning(ctx.Exception, "JWT authentication failed");
                        return Task.CompletedTask;
                    },
                    OnChallenge = ctx =>
                    {
                        // Burada da log yaza bilərsən istəsən
                        return Task.CompletedTask;
                    }
                };

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwt.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwt.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero, // testdə rahat debugging edir

                    NameClaimType = ClaimTypes.NameIdentifier,
                    RoleClaimType = ClaimTypes.Role
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("StudentOnly", p => p.RequireRole("Student"));
            options.AddPolicy("TeacherOnly", p => p.RequireRole("Teacher"));
            options.AddPolicy("AnyAppUser", p => p.RequireRole("Student", "Teacher"));
        });
    }
}