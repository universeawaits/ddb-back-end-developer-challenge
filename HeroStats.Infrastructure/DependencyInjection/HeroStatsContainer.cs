using DryIoc;
using HeroStats.Infrastructure.DataAccess;
using HeroStats.Infrastructure.Hero;

namespace HeroStats.Infrastructure.DependencyInjection;

public static class HeroStatsContainer
{
    public static IContainer Build() => (IContainer)CreateDefaultContainer().Database().Hero();

    private static IContainer CreateDefaultContainer() => new Container(x => x.WithDefaultReuse(Reuse.ScopedOrSingleton));
}
