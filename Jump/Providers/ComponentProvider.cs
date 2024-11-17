using System.Reflection;
using Jump.Attributes.Components;
using Jump.Exceptions;

namespace Jump.Providers;

/// <summary>
/// Class <c>ComponentProvider</c> is a provider for components.
/// You can use this to get a component from the store.
/// </summary>
public sealed class ComponentProvider
{

    private readonly Dictionary<Type, object> _components = new();
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
        return componentType.CustomAttributes.Any(attr => attr.AttributeType == typeof(Singleton)) 
            ? _components[componentType] : CreateInstance(componentType);
    }

    private object CreateInstance(Type type)
    {
        var constructor = GetConstructor(type);

        var parameters = constructor.GetParameters()
            .Select(p => GetComponent(p.ParameterType))
            .ToArray();

        return constructor.Invoke(parameters);
    }

    private ConstructorInfo GetConstructor(Type type)
    {
        var constructors = type.GetConstructors();
        var sortedConstructors = constructors.OrderByDescending(c => c.GetParameters().Length)
            .ToList();
        
        if (sortedConstructors.Count > 1 && sortedConstructors[0].GetParameters().Length ==
            sortedConstructors[1].GetParameters().Length)
            throw new AmbiguousMatchException($"Type ${type.Name} has multiple valid constructors.");

        var constructor = sortedConstructors.FirstOrDefault();
        
        if(constructor == null) throw new InvalidComponentException("No valid constructors found for " + type.Name);

        return constructor;
    }
    
    private void AddSingleton(object component)
    {
        if(_components.ContainsKey(component.GetType())) return;
        _components.Add(component.GetType(), component);
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