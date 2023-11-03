using System.Diagnostics;
using System.Reflection;
using MongoDB.Driver;

namespace HeroStats.Infrastructure.DataAccess.MongoDb;

public class MongoContext : IDatabaseContext<IMongoDatabase>
{
    public IMongoDatabase Database { get; }

    private static string? _applicationName;
    private static string ApplicationName => _applicationName ??=
        Assembly.GetEntryAssembly()?.ToString().Split(",")[0] ?? Process.GetCurrentProcess().ProcessName;

    protected MongoContext(DatabaseSettings settings)
    {
        var mongoClientSettings = MongoClientSettings.FromConnectionString(settings.ConnectionString);
        mongoClientSettings.ApplicationName = ApplicationName;

        Database = new MongoClient(mongoClientSettings).GetDatabase(settings.DatabaseName);
    }
}