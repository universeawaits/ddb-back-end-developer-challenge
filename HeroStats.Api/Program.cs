using HeroStats.Api;
using HeroStats.Api.Hero.Parser;
using HeroStats.Domain.Hero;
using HeroStats.Infrastructure.DataAccess;

var builder = WebApplication.CreateBuilder(args).ConfigureBuilder();
var app = builder.Build().ConfigureApplication();
FillDb(app);

await app.RunAsync();

// functions
void FillDb(IHost host)
{
    var heroes = new List<object>();
    Directory.GetFiles("Assets/Hero").AsParallel().ForAll(async x =>
    {
        using var streamReader = new StreamReader(x);
        var fileContent = await streamReader.ReadToEndAsync();
        heroes.Add(HeroParser.Parse(fileContent));
    });

    var db = host.Services.GetRequiredService<IDatabaseContext<IDictionary<Type, ICollection<object>>>>();
    db.Database.Add(typeof(Hero), heroes);
}
