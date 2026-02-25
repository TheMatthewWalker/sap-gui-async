using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class AppConfig
{
    public string ApiBaseUrl { get; set; }
    public string ApiKey { get; set; }
    public List<string> ShiftOptions { get; set; }
    public List<string> SapSystems { get; set; }

    public static AppConfig Load()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

        if (!File.Exists(path))
            throw new FileNotFoundException($"Config file not found at: {path}");

        var json = File.ReadAllText(path);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var config = JsonSerializer.Deserialize<AppConfig>(json, options);

        if (config == null)
            throw new Exception($"Failed to deserialise appsettings.json. Contents: {json}");

        return config;
    }




}

