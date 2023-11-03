using HeroStats.Application.RequestPipeline;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace HeroStats.Infrastructure.DependencyInjection;

public static class MediatrConfiguration
{
    public static void RegisterMediatR(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(typeof(HealRequest));
        serviceCollection.AddMediatR(typeof(DamageRequest));
    }
}
