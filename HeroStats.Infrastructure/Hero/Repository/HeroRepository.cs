using HeroStats.Domain.Hero;
using HeroStats.Infrastructure.DataAccess;
using MongoDB.Driver;

namespace HeroStats.Infrastructure.Hero.Repository;

public class HeroRepository : IHeroRepository
{
    private readonly IDatabaseContext<IMongoDatabase> _dbContext;

    public HeroRepository(IDatabaseContext<IMongoDatabase> dbContext) => _dbContext = dbContext;

    public void Create(Domain.Hero.Hero hero) => GetMongoCollection().InsertOne(hero);

    public Domain.Hero.Hero? Get(string name) => GetMongoCollection().Find(x => x.Name == name).SingleOrDefault();

    public void Update(Domain.Hero.Hero hero) => GetMongoCollection().ReplaceOne(x => x.Name == hero.Name, hero);

    private IMongoCollection<Domain.Hero.Hero> GetMongoCollection() =>
        _dbContext.Database.GetCollection<Domain.Hero.Hero>(nameof(Domain.Hero.Hero));
}
