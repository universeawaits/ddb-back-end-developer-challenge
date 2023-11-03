using DryIoc;
using HeroStats.Infrastructure.DataAccess;
using HeroStats.Infrastructure.Hero;
using Microsoft.Extensions.Configuration;

namespace HeroStats.Infrastructure.DependencyInjection;

public static class HeroStatsContainer
{
    public static IContainer Build(IConfiguration configuration) => (IContainer)CreateDefaultContainer()
        .Database(configuration).Hero();

    private static IContainer CreateDefaultContainer() => new Container(x => x.WithDefaultReuse(Reuse.ScopedOrSingleton));
}
