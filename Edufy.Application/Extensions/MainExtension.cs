using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Edufy.Application.Extensions;

public static class MainExtension
{
    public static void AddMainExtension(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEfCore(configuration);
        services.AddServices();
        services.AddJwtAuth(configuration);
        services.AddMediatr();
    }
}