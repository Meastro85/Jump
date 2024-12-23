namespace Jump.Attributes.Cache;

/// <summary>
/// The cache evict attribute is used to invalidate a cache.
/// This will remove the value from the cache.
/// You can create a cached value by using the <see cref="Cacheable"/> attribute on another method.
/// </summary>
/// <param name="key">The primary key for the cache.</param>
/// <param name="paramNames">The parameter names that should be used in the cache key.</param>
[AttributeUsage(AttributeTargets.Method)]
public class CacheEvict(string key, params string[]? paramNames) : Interceptor
{
    public string Key { get; } = key;
    public string[]? ParamNames { get; } = paramNames;
}