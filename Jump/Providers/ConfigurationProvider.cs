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
    private readonly HopProvider _hopProvider = HopProvider.Instance;

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

    public static ConfigurationProvider Dispose()
    {
        HopProvider.Dispose();
        _instance = null;
        return Instance;
    }

    private void AddConfiguration(object configuration)
    {
        if (_configurations.ContainsKey(configuration.GetType()))
        {
            Logging.LogWarning($"Configuration {configuration.GetType()} already exists.");
            return;
        }

        _configurations.Add(configuration.GetType(), configuration);
        Logging.LogInformation($"Configuration {configuration.GetType()} added.");
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

    private object CreateConfiguration(Type type, PropertiesFileParser propertiesParser)
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

        _hopProvider.AddHops(type);

        return configuration!;
    }

    public object GetHop(Type hop)
    {
        return _hopProvider.GetHop(hop);
    }

    internal bool IsHop(Type type)
    {
        return _hopProvider.IsHop(type);
    }
}