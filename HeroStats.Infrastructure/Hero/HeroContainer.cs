using DryIoc;
using HeroStats.Application.RequestPipeline;
using HeroStats.Domain.Hero.DataAccess;
using HeroStats.Infrastructure.Hero.Repository;
using MediatR;

namespace HeroStats.Infrastructure.Hero;

public static class HeroContainer
{
    public static IRegistrator Hero(this IRegistrator registrator)
    {
        registrator.Register<IHeroRepository, HeroRepository>(Reuse.Scoped);

        registrator.Register<IRequestHandler<HealRequest>, HealRequestHandler>(Reuse.Scoped);
        registrator.Register<IRequestHandler<TemporaryHealRequest>, TemporaryHealRequestHandler>(Reuse.Scoped);
        registrator.Register<IRequestHandler<DamageRequest>, DamageRequestHandler>(Reuse.Scoped);

        return registrator;
    }
}
