using HeroStats.Domain.Hero.DataAccess;
using MediatR;

namespace HeroStats.Application.Hero;

public class TemporaryHealRequestHandler : IRequestHandler<TemporaryHealRequest>
{
    private readonly IHeroRepository _repository;

    public TemporaryHealRequestHandler(IHeroRepository repository) => _repository = repository;

    public Task Handle(TemporaryHealRequest request, CancellationToken cancellationToken)
    {
        var hero = _repository.Get(request.Hero);

        var oldHp = hero.TemporaryHp;
        hero.TemporaryHp = Math.Max(hero.TemporaryHp, request.Points);
        if (hero.TemporaryHp != oldHp)
            _repository.Update(hero);

        return Task.CompletedTask;
    }
}
