using Microsoft.Extensions.DependencyInjection;

namespace Edufy.Application.Extensions;

public static class RegisterStrategyPattern
{
    // public static void AddStrategies(this IServiceCollection services)
    // {
    //     services.AddScoped<IBalanceToBalanceStrategy, IbanToIbanStrategy>();
    //     services.AddScoped<IBalanceToBalanceStrategy, IbanToUIdStrategy>();
    //     services.AddScoped<IBalanceToBalanceStrategy, UIdToIbanStrategy>();
    //     services.AddScoped<IBalanceToBalanceStrategy, UIdToUIdStrategy>();
    //     services.AddScoped<IBalanceToBalanceStrategy, IbanToPhoneNumberStrategy>();
    //     services.AddScoped<IBalanceToBalanceStrategy, PhoneNumberPayoutStrategy>();
    //     services.AddScoped<IBalanceToBalanceStrategy, IbanPayoutStrategy>();
    //     services.AddScoped<IBalanceToBalanceStrategy, UIdPayoutStrategy>();
    //     var strategyType = typeof(IBalanceToBalanceStrategy);
    //     var strategies = strategyType.Assembly.GetTypes()
    //         .Where(type => strategyType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);
    //
    //     foreach (var strategy in strategies)
    //         services.AddScoped(strategyType, strategy);
    // }
}