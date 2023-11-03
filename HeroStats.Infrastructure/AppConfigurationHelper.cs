using Microsoft.Extensions.Configuration;

namespace HeroStats.Infrastructure;

public static class AppConfigurationHelper
{
    public static IConfiguration GetAppConfiguration()
    {
        var configurationBuilder = new ConfigurationBuilder();
        var fullConfigurationFilePath = FindFileUpwards(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

        return string.IsNullOrEmpty(fullConfigurationFilePath) 
            ? configurationBuilder.Build() 
            : configurationBuilder.AddJsonFile(fullConfigurationFilePath).Build();
    }

    private static string? FindFileUpwards(string? directory, string filename)
    {
        do
        {
            var fullName = Path.Combine(directory!, filename);
            if (File.Exists(fullName))
                return fullName;

            if (directory!.Contains(Path.DirectorySeparatorChar) == false)
                return null;

            directory = Path.GetDirectoryName(directory);
        } while (directory != null);

        return null;
    }
}
