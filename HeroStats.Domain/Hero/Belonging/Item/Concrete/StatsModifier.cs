namespace HeroStats.Domain.Hero.Belonging.Item.Concrete;

public class StatsModifier : ItemModifier
{
    public StatsType Target { get; init; }
    public int Value { get; init; }
    
    public override void Modify(Hero hero)
    {
        var resultValue = hero.Stats[Target] + Value;
        if (resultValue >= 0)
            hero.Stats[Target] = (uint)resultValue;
    }
}
