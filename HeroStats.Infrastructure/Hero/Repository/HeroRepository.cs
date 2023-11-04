using HeroStats.Domain.Hero.DataAccess;
using HeroStats.Infrastructure.DataAccess;

namespace HeroStats.Infrastructure.Hero.Repository;

public class HeroRepository : IHeroRepository
{
    private readonly IDatabaseContext<IDictionary<Type, ICollection<object>>> _dbContext;

    public HeroRepository(IDatabaseContext<IDictionary<Type, ICollection<object>>> dbContext) => _dbContext = dbContext;

    public Domain.Hero.Hero Get(string name)
    {
        var hero = _dbContext.Database[typeof(Domain.Hero.Hero)]
            .Cast<Domain.Hero.Hero>().SingleOrDefault(x => x.Name == name);
        if (hero is null)
            throw new HeroNotFoundException(name);

        return hero;
    }

    public void Update(Domain.Hero.Hero hero)
    {
        // nothing to do, updated as already retrieved by reference
    }
}
