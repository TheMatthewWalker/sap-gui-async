using System;
using System.IO;
using System.Text.Json;

public class AppConfig
{
    public string ApiBaseUrl { get; set; }
    public string ApiKey { get; set; }

    public static AppConfig Load()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

        if (!File.Exists(path))
            throw new FileNotFoundException($"Config file not found at {path}");

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<AppConfig>(json);
    }
}