using Edufy.Application.Abstractions;
using Edufy.Application.Services;
using Edufy.Domain.Abstractions;
using Edufy.Domain.Entities;
using Edufy.Domain.Services;
using Edufy.SqlServer.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Edufy.Application.Extensions;

public static class ServiceRegistration
{
    public static void AddServices(this IServiceCollection service)
    {
        service.AddHttpContextAccessor();
        service.AddScoped<ICurrentUser, CurrentUser>();
        service.AddScoped<ITokenService, TokenService>();
        service.AddScoped<IAuthService, AuthService>();

        service.AddIdentityCore<User>(opt => { opt.User.RequireUniqueEmail = true; })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<EdufyDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();
    }
}