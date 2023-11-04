using HeroStats.Domain.Action.Damage;

namespace HeroStats.Domain.Hero;

public class Hero
{
    public string Name { get; init; }

    #region Hit points // TODO extract to a separate class; here exists only for serialization ease reasons

    public uint HitPoints { get; init; }
    public uint CurrentPersistent { get; private set; }
    public uint Temporary { get; set; }

    public void SetCurrentPersistent(uint value)
    {
        if (value <= HitPoints)
            CurrentPersistent = Math.Max(value, 0);
    }

    #endregion

    public ICollection<DamageDefense> Defenses { get; init; } = new List<DamageDefense>();
}
