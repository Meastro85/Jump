using System.Reflection;
using Jump.Attributes;
using Jump.Exceptions;

namespace Jump.Providers;

public class HopProvider
{
    private static HopProvider? _instance;
    private static readonly object Padlock = new();
    private readonly Lazy<ComponentProvider> _componentProvider = new( () => ComponentProvider.Instance);
    private readonly Dictionary<Type, object> _hops = new();

    public static HopProvider Instance
    {
        get
        {
            lock (Padlock)
            {
                return _instance ??= new HopProvider();
            }
        }
    }

    private void AddHop(Type hop, object instance)
    {
        if (!_hops.TryAdd(hop, instance))
        {
            throw new DuplicateHopException("The hop: " + hop.Name + " is already registered. Check your configuration.");
        }
    }

    private void CreateHop(MethodInfo hopDefinition)
    {
        var parameters = hopDefinition.GetParameters()
            .Select(p => _componentProvider.Value.GetComponent(p.ParameterType))
            .ToArray();
        var returnType = hopDefinition.ReturnParameter.ParameterType;

        var instance = hopDefinition.Invoke(null, parameters)!;
        AddHop(returnType, instance);
    }

    internal void AddHops(Type configuration)
    {
        configuration.GetMethods()
            .Where(m => m.GetCustomAttributes(true)
                .Any(attr => attr is Hop))
            .ToList()
            .ForEach(CreateHop);
    }

    internal bool IsHop(Type type)
    {
        return _hops.ContainsKey(type);
    }

    internal object GetHop(Type hop)
    {
        return _hops[hop];
    }
}