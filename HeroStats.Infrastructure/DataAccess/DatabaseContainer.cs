using DryIoc;
using HeroStats.Infrastructure.DataAccess.MongoDb;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace HeroStats.Infrastructure.DataAccess;

public static class DatabaseContainer
{
    private const string DatabaseName = "HeroStats";
    private const string ConnectionString = nameof(ConnectionString);

    public static IRegistrator Database(this IRegistrator registrator, IConfiguration configuration)
    {
        registrator.RegisterInstance(new DatabaseSettings
        {
            ConnectionString = configuration[ConnectionString]!,
            DatabaseName = DatabaseName,
        });

        registrator.Register<IDatabaseContext<IMongoDatabase>, MongoContext>(Reuse.Singleton);

        return registrator;
    }
}
