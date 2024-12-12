using Castle.DynamicProxy;
using Jump.Attributes.Cache;
using Jump.Providers;

namespace Jump.Interceptors;

public class CacheEvictInterceptor : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        if (!invocation.Method.GetCustomAttributes(false).Any(attr => attr is CacheEvict))
        {
            invocation.Proceed();
            return;
        }
        var attribute = invocation.Method.GetCustomAttributes(false).First(attr => attr is CacheEvict) as CacheEvict;

        var key = Utility.CreateCacheKey(attribute!.Key, invocation.Arguments);
        var cacheProvider = CacheProvider.Instance;
        cacheProvider.RemoveFromCache(key);
        invocation.Proceed();
    }
}