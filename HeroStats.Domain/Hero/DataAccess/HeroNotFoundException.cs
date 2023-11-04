namespace HeroStats.Domain.Hero.DataAccess;

public class HeroNotFoundException : Exception
{
    private readonly string _name;

    public HeroNotFoundException(string name) => _name = name;

    public override string Message => $"Hero {_name} doesn't exist";
}
