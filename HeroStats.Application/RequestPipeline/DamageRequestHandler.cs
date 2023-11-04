using HeroStats.Domain.Action.Damage;
using HeroStats.Domain.Hero;
using HeroStats.Domain.Hero.DataAccess;
using MediatR;

namespace HeroStats.Application.RequestPipeline;

public class DamageRequestHandler : IRequestHandler<DamageRequest>
{
    private readonly IHeroRepository _repository;

    public DamageRequestHandler(IHeroRepository repository) => _repository = repository;

    public Task Handle(DamageRequest request, CancellationToken cancellationToken)
    {
        var hero = _repository.Get(request.Hero);
        var defense = ApplicableDefense(request, hero);
        ApplyDamageIfPossible(request, defense, hero);

        return Task.CompletedTask;
    }

    private void ApplyDamageIfPossible(DamageRequest request, uint defense, Hero hero)
    {
        var resultingDamage = defense >= request.Amount ? 0 : request.Amount - defense;
        if (resultingDamage is 0)
            return;

        ApplyDamage(resultingDamage, hero);
        _repository.Update(hero);
    }

    private static void ApplyDamage(uint damage, Hero hero)
    {
        var damageToTemporaryHp = Math.Min(hero.Temporary, damage);
        damage -= damageToTemporaryHp;

        var damageToPersistentHp = Math.Min(hero.CurrentPersistent, damage);

        hero.Temporary -= damageToTemporaryHp;
        hero.SetCurrentPersistent(hero.CurrentPersistent - damageToPersistentHp);
    }

    private static uint ApplicableDefense(DamageRequest request, Hero hero)
    {
        var defense = hero.Defenses.SingleOrDefault(x => x.Type == request.Type);
        var applicableDefense = defense?.Defense switch
        {
            DefenseType.Immunity => request.Amount,
            DefenseType.Resistance => request.Amount / 2,
            null => 0u,
            _ => throw new ArgumentOutOfRangeException($"Couldn't handle {defense.Defense} defense type"),
        };

        return applicableDefense;
    }
}
