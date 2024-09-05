using Microsoft.Extensions.Configuration;

ConfigurationBuilder builder = new ConfigurationBuilder();
builder.AddText("appsettings.txt");
IConfiguration configuration = builder.Build();

string LoggerLevel = configuration.GetValue<string>("Logging.LogLevel.Default");
string AppplicationName = configuration.GetValue<string>("ApplicationSettings.AppName");
string ApplicationVerison = configuration.GetValue<string>("ApplicationSettings.Version");

Console.WriteLine($"Logging.LogLevel.Default => {LoggerLevel}");
Console.WriteLine($"ApplicationSettings.AppName =>  { AppplicationName}");
Console.WriteLine($"ApplicationSettings.Version =>  { ApplicationVerison}");

public static class TextConfigurationExtensions
{
    public static IConfigurationBuilder AddText(this IConfigurationBuilder configurationBuilder, string FilePath)
    {
        TextConfigurationSource source = new TextConfigurationSource(FilePath);
        return configurationBuilder.Add(source);
    }
}

public class TextConfigurationProvider: ConfigurationProvider
{
    public TextConfigurationSource Source { get; set; }
    public TextConfigurationProvider(TextConfigurationSource source)
    {
        this.Source = source;
    }

    public override void Load()
    {
        var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        if (!File.Exists(Source.FilePath))
        {
            throw new FileNotFoundException($"Configuration 文件 '{Source.FilePath}' 没有发现");
        }

        var lines = File.ReadAllLines(Source.FilePath);

        foreach (var line in lines)
        {
            var keyValuePair = line.Split('=');
            if (keyValuePair.Length == 2)
            {
                data.Add(keyValuePair[0].Trim(), keyValuePair[1].Trim());
            }
        }

        this.Data = data;
    }
}

public class TextConfigurationSource: IConfigurationSource
{
    public string FilePath { get; set; }

    public TextConfigurationSource(string filePath)
    {
        this.FilePath = filePath;
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new TextConfigurationProvider(this);
    }
}