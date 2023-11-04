using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace HeroStats.Infrastructure.DependencyInjection;

public static class MediatrConfiguration
{
    public static void RegisterMediatR(this IServiceCollection services) =>
        services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
}
