using System.Reflection;
using Jump.Exceptions;

namespace Jump;

public sealed class ComponentStore
{

    private static ComponentStore? _instance;
    private static readonly object Padlock = new();
    private readonly IDictionary<Type, ICollection<Type>> _components = new Dictionary<Type, ICollection<Type>>();
    private readonly IDictionary<Type, ICollection<ConstructorInfo>> _constructors = new Dictionary<Type, ICollection<ConstructorInfo>>();

    public static ComponentStore Instance
    {
        get
        {
            lock (Padlock)
            {
                if (_instance == null) _instance = new ComponentStore();
                return _instance;
            }
        }
    }
    
    public void AddComponent(Type component)
    {
        var attributeData = component.CustomAttributes
            .Where(attr => Utility.InheritsFromComponent(attr.AttributeType))
            .ToList();
        
        if(attributeData.Count > 1) throw new TooManyAttributesException("Multiple component attributes on a single class is not allowed");
            
        var componentType = attributeData
            .First(attr => Utility.InheritsFromComponent(attr.AttributeType))
            .AttributeType;
        
        if (_components.TryGetValue(componentType, out var list)) list.Add(component);
        else _components.Add(componentType, [component]);
        
        AddConstructor(component, component.GetConstructors());;
    }

    public void AddConstructor(Type componentType, ConstructorInfo[] constructorInfo)
    {
        _constructors.Add(componentType, constructorInfo.ToList());
    }

    public IDictionary<Type, ICollection<Type>> GetComponents()
    {
        return _components;
    }

    public IDictionary<Type, ICollection<ConstructorInfo>> GetConstructors()
    {
        return _constructors;
    }
    
}