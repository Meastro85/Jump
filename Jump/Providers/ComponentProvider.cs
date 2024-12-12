using Jump.Attributes.Components;

namespace Jump.Providers;

/// <summary>
/// Class <c>ComponentProvider</c> is a provider for components.
/// You can use this to get a component from the store.
/// </summary>
public sealed class ComponentProvider
{

    private readonly Dictionary<Type, object> _singletons = new();
    private readonly ComponentStore _componentStore = ComponentStore.Instance;
    private static ComponentProvider? _instance;
    private static readonly object Padlock = new();
    
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
    
    /// <summary>
    /// This method gets creates an instance of a component, or a singleton if it exists.
    /// </summary>
    /// <param name="componentType">The type of the component you want to get.</param>
    /// <returns>Your requested component.</returns>
    public object GetComponent(Type componentType)
    {
        var isSingleton = componentType.CustomAttributes.Any(attr => attr.AttributeType == typeof(Singleton));
        
        if (isSingleton) return _singletons[componentType];
        return CreateInstance(componentType);
    }

    private object CreateInstance(Type type)
    {
        var constructor = _componentStore.GetConstructor(type);

        var parameters = _componentStore.GetParameters(constructor)
            .Select(p => GetComponent(p.ParameterType))
            .ToArray();

        return constructor.Invoke(parameters);
    }

    
    
    private void AddSingleton(object component)
    {
        if(_singletons.ContainsKey(component.GetType())) return;
        _singletons.Add(component.GetType(), component);
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