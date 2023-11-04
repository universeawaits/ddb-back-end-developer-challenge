using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeroStats.Api.Hero.Parser;

public static class HeroParser
{
    public static Domain.Hero.Hero Parse(string json)
    {
        var hero = JsonSerializer.Deserialize<Domain.Hero.Hero>(json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter() }
            })!;
        hero.SetCurrentPersistent(hero.HitPoints);

        return hero;
    }
}
