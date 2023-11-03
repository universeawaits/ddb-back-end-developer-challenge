namespace HeroStats.Domain.Hero;

public interface IHeroRepository
{
    void Create(Hero hero);
    Hero? Get(string name);
    void Update(Hero hero);
}
