namespace HeroStats.Infrastructure.DataAccess;

public interface IDatabaseContext<out T>
{
    T Database { get; }
}
