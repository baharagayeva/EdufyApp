using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Edufy.Application.Extensions;

public static class MainExtension
{
    public static void AddMainExtension(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEfCore(configuration);
        services.AddServices();
        services.AddAuth(configuration);
        services.AddMediatr();
        // services.AddAutoMapper();
        // services.AddStrategies();
        // services.AddFactories();
    }
}