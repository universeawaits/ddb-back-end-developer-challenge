using DryIoc;
using HeroStats.Infrastructure.DataAccess.Concrete;

namespace HeroStats.Infrastructure.DataAccess;

public static class DatabaseContainer
{
    public static IRegistrator Database(this IRegistrator registrator)
    {
        registrator.Register<IDatabaseContext<IDictionary<Type, ICollection<object>>>, InMemoryDatabaseContext>(Reuse.Singleton);

        return registrator;
    }
}
