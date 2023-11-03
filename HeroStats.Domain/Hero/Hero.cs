using HeroStats.Domain.Action.Damage;
using HeroStats.Domain.Action.Health;
using HeroStats.Domain.Hero.Belonging.Item;
using HeroStats.Domain.Hero.Class;

namespace HeroStats.Domain.Hero;

public class Hero
{
    public string Name { get; init; }
    public uint Level { get; init; } // assuming permanent as per test task
    public HitPoints HitPoints { get; private set; }
    
    public IDictionary<StatsType, uint> Stats { get; init; }

    public ICollection<Item> Items { get; init; }
    public ICollection<Defense> Defenses { get; init; }
    public ISet<HeroClass> Classes { get; init; }
}
