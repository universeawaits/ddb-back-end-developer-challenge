namespace HeroStats.Infrastructure.DataAccess.Concrete;

public class InMemoryDatabaseContext : IDatabaseContext<IDictionary<Type, ICollection<object>>>
{
    public IDictionary<Type, ICollection<object>> Database { get; } = new Dictionary<Type, ICollection<object>>();
}
