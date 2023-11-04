using HeroStats.Domain.Hero.DataAccess;
using MediatR;

namespace HeroStats.Application.RequestPipeline;

public class TemporaryHealRequestHandler : IRequestHandler<TemporaryHealRequest>
{
    private readonly IHeroRepository _repository;

    public TemporaryHealRequestHandler(IHeroRepository repository) => _repository = repository;

    public Task Handle(TemporaryHealRequest request, CancellationToken cancellationToken)
    {
        var hero = _repository.Get(request.Hero);
        hero.Temporary = Math.Max(hero.Temporary, request.Points);
        _repository.Update(hero);

        return Task.CompletedTask;
    }
}
