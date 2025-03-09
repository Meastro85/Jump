using Jump.LoggingSetup;

namespace Jump.Providers;

public sealed class CacheProvider
{
    private readonly Dictionary<string, object> _cache = new();
    private static CacheProvider? _instance;
    private static readonly object Padlock = new();
    
    public static CacheProvider Instance
    {
        get
        {
            lock (Padlock)
            {
                return _instance ??= new CacheProvider();
            }
        }
    }

    public static CacheProvider Dispose()
    {
        _instance = null;
        return Instance;
    }
    
    internal void AddToCache(string key, object value)
    {
        Logging.LogInformation($"Adding {key} to cache with value: {value}");
        if(!_cache.TryAdd(key, value))
            Logging.LogWarning($"Key {key} already exists in cache");
    }

    internal void RemoveFromCache(string key)
    {
        if(_cache.Remove(key))
            Logging.LogInformation($"Key {key} removed from cache");
        else
            Logging.LogWarning($"Key {key} not found in cache");
    }

    internal bool TryGetFromCache(string key, out object? value)
    {
        Logging.LogInformation($"Trying to get {key} from cache");
        return _cache.TryGetValue(key, out value);
    }
    
}