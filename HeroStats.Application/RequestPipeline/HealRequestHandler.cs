using HeroStats.Domain.Hero.DataAccess;
using MediatR;

namespace HeroStats.Application.RequestPipeline;

public class HealRequestHandler : IRequestHandler<HealRequest>
{
    private readonly IHeroRepository _repository;

    public HealRequestHandler(IHeroRepository repository) => _repository = repository;

    public Task Handle(HealRequest request, CancellationToken cancellationToken)
    {
        var hero = _repository.Get(request.Hero);

        var oldHp = hero.CurrentPersistent;
        hero.SetCurrentPersistent(hero.CurrentPersistent + request.Points);
        if (oldHp != hero.CurrentPersistent)
            _repository.Update(hero);

        return Task.CompletedTask;
    }
}
