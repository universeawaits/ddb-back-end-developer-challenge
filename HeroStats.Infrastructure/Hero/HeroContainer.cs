using DryIoc;
using HeroStats.Domain.Hero;
using HeroStats.Infrastructure.Hero.Repository;

namespace HeroStats.Infrastructure.Hero;

public static class HeroContainer
{
    public static IRegistrator Hero(this IRegistrator registrator)
    {
        registrator.Register<IHeroRepository, HeroRepository>();

        return registrator;
    }
}
