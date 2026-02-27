using Edufy.Application.CQRS.Queries.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace Edufy.Application.Extensions;

public static class CqrsService
{
    public static void AddMediatr(this IServiceCollection service)
    {
        service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetHomeQueryRequest).Assembly));
    }
}