using HeroStats.Domain.Action.Damage;
using HeroStats.Domain.Hero.DataAccess;
using MediatR;

namespace HeroStats.Application.Hero;

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

    private void ApplyDamageIfPossible(DamageRequest request, uint defense, Domain.Hero.Hero hero)
    {
        var resultingDamage = defense >= request.Amount ? 0 : request.Amount - defense;
        if (resultingDamage is 0)
            return;

        ApplyDamage(resultingDamage, hero);
        _repository.Update(hero);
    }

    private static void ApplyDamage(uint damage, Domain.Hero.Hero hero)
    {
        var damageToTemporaryHp = Math.Min(hero.TemporaryHp, damage);
        damage -= damageToTemporaryHp;

        var damageToPersistentHp = Math.Min(hero.CurrentPersistentHp, damage);

        hero.TemporaryHp -= damageToTemporaryHp;
        hero.SetCurrentPersistent(hero.CurrentPersistentHp - damageToPersistentHp);
    }

    private static uint ApplicableDefense(DamageRequest request, Domain.Hero.Hero hero)
    {
        var hasDefense = hero.Defenses.TryGetValue(request.Type, out var defense);
        if (!hasDefense)
            return 0u;

        var applicableDefense = defense switch
        {
            DefenseType.Immunity => request.Amount,
            DefenseType.Resistance => request.Amount / 2,
            _ => throw new ArgumentOutOfRangeException($"Couldn't handle {defense} defense type"),
        };

        return applicableDefense;
    }
}
