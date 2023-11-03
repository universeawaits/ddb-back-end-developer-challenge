using HeroStats.Domain.Hero;
using MediatR;

namespace HeroStats.Application.RequestPipeline;

public class HealRequestHandler : IRequestHandler<HealRequest>
{
    private readonly IHeroRepository _repository;

    public HealRequestHandler(IHeroRepository repository) => _repository = repository;

    public Task Handle(HealRequest request, CancellationToken cancellationToken)
    {
        var hero = _repository.Get(request.Hero);
        if (hero is null)
            throw new HeroNotFoundException(request.Hero);

        hero.HitPoints.SetCurrentPersistent(hero.HitPoints.CurrentPersistent + request.Points);
        _repository.Update(hero);

        return Task.CompletedTask;
    }
}

public class HeroNotFoundException : Exception
{
    private readonly string _name;

    public HeroNotFoundException(string name) => _name = name;

    public override string Message => $"Hero {_name} doesn't exist";
}
