using Edufy.Domain.DTOs.AuthDTOs;
using Edufy.SqlServer.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Edufy.Application.Extensions;

public static class OtherService
{
    public static void AddEfCore(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddDbContext<EdufyDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Postgres"),
                nmpOptions => { nmpOptions.MigrationsAssembly("Edufy.SqlServer"); });
        });
    }
    
    public static void AddSmtp(this IServiceCollection service, IConfiguration configuration)
    {
        service.Configure<SmtpOptions>(configuration.GetSection("Smtp"));
    }
}