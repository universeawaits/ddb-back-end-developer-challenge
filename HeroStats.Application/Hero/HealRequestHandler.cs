using HeroStats.Domain.Hero.DataAccess;
using MediatR;

namespace HeroStats.Application.Hero;

public class HealRequestHandler : IRequestHandler<HealRequest>
{
    private readonly IHeroRepository _repository;

    public HealRequestHandler(IHeroRepository repository) => _repository = repository;

    public Task Handle(HealRequest request, CancellationToken cancellationToken)
    {
        var hero = _repository.Get(request.Hero);

        var oldHp = hero.CurrentPersistentHp;
        hero.SetCurrentPersistent(hero.CurrentPersistentHp + request.Points);
        if (oldHp != hero.CurrentPersistentHp)
            _repository.Update(hero);

        return Task.CompletedTask;
    }
}
