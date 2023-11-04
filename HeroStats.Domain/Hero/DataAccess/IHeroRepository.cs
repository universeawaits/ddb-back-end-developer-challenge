namespace HeroStats.Domain.Hero.DataAccess;

public interface IHeroRepository
{
    Hero Get(string name);
    void Update(Hero hero);
}
