using System.Reflection;
using Jump.Attributes.Components;
using Jump.Exceptions;

namespace Jump.Providers;

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
    
    public object GetComponent(Type componentType)
    {
        return componentType.CustomAttributes.Any(attr => attr.GetType() == typeof(Singleton)) 
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
    
    internal void AddComponent(object component)
    {
        
    }

    internal void AddComponents(ICollection<Type> components)
    {
        foreach (var componentType in components)
        {
            
        }
    }
    
}