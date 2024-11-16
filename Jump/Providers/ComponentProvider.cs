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
        return _components[componentType];
    }

    internal void AddComponent(object component)
    {
        
    }

    internal void AddComponents(ICollection<Type> components)
    {
        
    }
    
}