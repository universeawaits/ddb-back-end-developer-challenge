namespace HeroStats.Domain.Hero.Class;

public abstract class HeroClass
{
    public abstract string Name { get; }
    public abstract uint HitDice { get; }
    public uint Level { get; init; } // assuming permanent as per test task 
}
