using System.Reflection;
using Jump.Attributes;
using Jump.LoggingSetup;
using Jump.Parsers;

namespace Jump.Providers;

public sealed class ConfigurationProvider
{
    private static ConfigurationProvider? _instance;
    private static readonly object Padlock = new();
    private readonly Dictionary<Type, object> _configurations = new();

    public static ConfigurationProvider Instance
    {
        get
        {
            lock (Padlock)
            {
                return _instance ??= new ConfigurationProvider();
            }
        }
    }

    private void AddConfiguration(object configuration)
    {
        if (_configurations.ContainsKey(configuration.GetType()))
        {
            Logging.Logger.LogWarning($"Configuration {configuration.GetType()} already exists.");
            return;
        }

        _configurations.Add(configuration.GetType(), configuration);
        Logging.Logger.LogInformation($"Configuration {configuration.GetType()} added.");
    }

    internal void AddConfigurations(IEnumerable<Type> configurations)
    {
        var properties = new PropertiesFileParser();
        foreach (var configuration in configurations) AddConfiguration(CreateConfiguration(configuration, properties));
    }

    public object GetConfiguration(Type configuration)
    {
        return _configurations[configuration];
    }

    private static object CreateConfiguration(Type type, PropertiesFileParser propertiesParser)
    {
        var configuration = Activator.CreateInstance(type);
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var prefixAttribute = type.GetCustomAttribute<ConfigurationProperties>();
        var prefix = prefixAttribute?.Prefix ?? string.Empty;
        foreach (var property in properties)
        {
            var key = $"{prefix}.{property.Name}";
            if (!propertiesParser.Keys.Contains(key)) continue;
            var value = Convert.ChangeType(propertiesParser.Get(key), property.PropertyType);
            property.SetValue(configuration, value);
        }

        return configuration!;
    }
}