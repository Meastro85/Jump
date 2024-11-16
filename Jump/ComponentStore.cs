using System.Reflection;
using Jump.Attributes.Components;
using Jump.Exceptions;

namespace Jump;

public sealed class ComponentStore
{

    private static ComponentStore? _instance;
    private static readonly object Padlock = new();
    private readonly IDictionary<Type, ICollection<Type>> _components = new Dictionary<Type, ICollection<Type>>();

    public static ComponentStore Instance
    {
        get
        {
            lock (Padlock)
            {
                return _instance ??= new ComponentStore();
            }
        }
    }
    
    public void AddComponent(Type component)
    {
        var attributeData = component.CustomAttributes
            .Where(attr => Utility.InheritsFromAttribute(attr.AttributeType, typeof(Component)))
            .ToList();
        
        if(attributeData.Count > 1) throw new TooManyAttributesException("Multiple component attributes on a single class is not allowed");
            
        var componentType = attributeData
            .First(attr => Utility.InheritsFromAttribute(attr.AttributeType, typeof(Component)))
            .AttributeType;
        
        if (_components.TryGetValue(componentType, out var list)) list.Add(component);
        else _components.Add(componentType, [component]);
    }

    public IDictionary<Type, ICollection<Type>> GetComponents()
    {
        return _components;
    }
    
}