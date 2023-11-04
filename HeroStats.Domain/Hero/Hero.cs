using HeroStats.Domain.Action.Damage;

namespace HeroStats.Domain.Hero;

public class Hero
{
    public string Name { get; init; }

    #region Hit points // TODO extract to a separate class; here exists only for serialization ease reasons

    public uint HitPoints { get; init; }
    public uint CurrentPersistentHp { get; private set; }
    public uint TemporaryHp { get; set; }

    public void SetCurrentPersistent(uint value)
    {
        var minToAssign = Math.Min(value, HitPoints);
        if (minToAssign <= HitPoints)
            CurrentPersistentHp = Math.Max(minToAssign, 0);
    }

    #endregion

    public IDictionary<DamageType, DefenseType> Defenses { get; } = new Dictionary<DamageType, DefenseType>();
}
