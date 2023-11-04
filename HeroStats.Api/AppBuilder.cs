using System.Text.Json.Serialization;
using DryIoc.Microsoft.DependencyInjection;
using HeroStats.Api.Middleware;
using HeroStats.Infrastructure;
using HeroStats.Infrastructure.DependencyInjection;

namespace HeroStats.Api;

public static class AppBuilder
{
    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddConfiguration(AppConfigurationHelper.GetAppConfiguration());
        var container = HeroStatsContainer.Build();

        builder.Host.UseServiceProviderFactory(new DryIocServiceProviderFactory(container));
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddControllers().AddJsonOptions(settings =>
        {
            settings.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
            settings.JsonSerializerOptions.AllowTrailingCommas = true;
            settings.JsonSerializerOptions.PropertyNameCaseInsensitive = true;

            settings.JsonSerializerOptions.Converters.Clear();
            settings.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        builder.Services.RegisterMediatR();

        return builder;
    }

    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseHttpsRedirection();
        app.MapControllers();

        return app;
    }
}
