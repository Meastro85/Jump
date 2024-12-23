using Castle.DynamicProxy;
using Jump.Attributes;
using Jump.Attributes.Cache;
using Jump.Attributes.Components;
using Jump.Interceptors;
using Jump.LoggingSetup;
using Jump.Providers.Component_store;

namespace Jump.Providers;

/// <summary>
///     Class <c>ComponentProvider</c> is a provider for components.
///     You can use this to get a component from the store.
/// </summary>
public sealed class ComponentProvider
{
    private static ComponentProvider? _instance;
    private static readonly object Padlock = new();
    private readonly ComponentStore _componentStore = ComponentStore.Instance;
    private readonly ConfigurationProvider _configurationProvider = ConfigurationProvider.Instance;
    private readonly Dictionary<Type, object> _singletons = new();

    public static ComponentProvider Instance
    {
        get
        {
            lock (Padlock)
            {
                return _instance ??= new ComponentProvider();
            }
        }
    }

    public static ComponentProvider Dispose()
    {
        _instance = null;
        return Instance;
    }

    /// <summary>
    ///     This method gets creates an instance of a component, or a singleton if it exists.
    /// </summary>
    /// <typeparam name="T">The type of the component you want to get.</typeparam>
    /// <returns>Your requested component.</returns>
    public T GetComponent<T>()
    {
        return (T)GetComponent(typeof(T));
    }

    /// <summary>
    ///     This method gets creates an instance of a component, or a singleton if it exists.
    /// </summary>
    /// <param name="componentType">The type of the component you want to get.</param>
    /// <returns>Your requested component.</returns>
    public object GetComponent(Type componentType)
    {
        var isSingleton = componentType.CustomAttributes.Any(attr => attr.AttributeType == typeof(Singleton));
        var isConfiguration = componentType.CustomAttributes.Any(attr => attr.AttributeType == typeof(Configuration));
        var isHop = _configurationProvider.IsHop(componentType);

        if (isHop) return _configurationProvider.GetHop(componentType);
        if (isSingleton) return _singletons[componentType];
        return isConfiguration ? _configurationProvider.GetConfiguration(componentType) : CreateInstance(componentType);
    }

    private object CreateInstance(Type type)
    {
        var constructor = _componentStore.GetConstructor(type);
        var parameters = _componentStore.GetParameters(constructor)
            .Select(p => GetComponent(p.ParameterType))
            .ToArray();

        if (type.GetMethods().Any(m => m.GetCustomAttributes(true).Any(attr => attr is Interceptor)))
            return GenerateProxy(type, parameters);

        if (Logging.LoggingLevel == LoggingLevel.DEBUG)
            Logging.Logger.LogInformation($"Created component: {type} with parameters: {parameters}");
        return constructor.Invoke(parameters);
    }

    private static object GenerateProxy(Type type, object[] parameters)
    {
        var proxyGenerator = new ProxyGenerator();
        var interceptionList = GetInterceptors(type);
        if (Logging.LoggingLevel == LoggingLevel.DEBUG)
            Logging.Logger.LogInformation($"Created component proxy: {type} with parameters: {parameters}");
        return proxyGenerator.CreateClassProxy(type, parameters, interceptionList.ToArray());
    }

    private static List<IInterceptor> GetInterceptors(Type type)
    {
        var attributes = type.GetMethods()
            .SelectMany(m => m.GetCustomAttributes(true));
        var interceptionList = new List<IInterceptor>();
        var enumerable = attributes.ToList();
        if (enumerable.Any(attr => attr is Cacheable)) interceptionList.Add(new CacheableInterceptor());
        if (enumerable.Any(attr => attr is CacheEvict)) interceptionList.Add(new CacheEvictInterceptor());
        return interceptionList;
    }

    private void AddSingleton(object component)
    {
        if (_singletons.ContainsKey(component.GetType()))
        {
            Logging.Logger.LogWarning($"Singleton of {component.GetType()} already exists.");
            return;
        }

        _singletons.Add(component.GetType(), component);
        Logging.Logger.LogInformation($"Singleton of {component.GetType()} added.");
    }

    internal void AddSingletons(ICollection<Type> components)
    {
        foreach (var componentType in components)
        {
            var component = CreateInstance(componentType);
            AddSingleton(component);
        }
    }
}