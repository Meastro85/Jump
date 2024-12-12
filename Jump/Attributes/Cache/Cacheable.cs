namespace Jump.Attributes.Cache;

/// <summary>
///     The cacheable attribute is used to mark methods that should be cached.
///     This will cache the return value of your method for later user.
///     You can invalidate this cache by using the <see cref="CacheEvict" /> attribute on another method.
/// </summary>
/// <param name="key">The primary key for the cache.</param>
/// <param name="paramNames">The parameter names that should be used in the cache key.</param>
[AttributeUsage(AttributeTargets.Method)]
public class Cacheable(string key, params string[]? paramNames) : Interceptor
{
    public string Key { get; } = key;
    public string[] ParamNames { get; } = paramNames;
}