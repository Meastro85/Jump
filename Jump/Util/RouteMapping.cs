using System.Reflection;

namespace Jump.Util;

internal class RouteMapping
{
    public IDictionary<object, ICollection<MethodMapping>> ControllerMapping { get; set; } =
        new Dictionary<object, ICollection<MethodMapping>>();

    public bool ActionExists(string action)
    {
        var methodMappings = ControllerMapping.Values.SelectMany(x => x).ToList();
        return methodMappings.Any(x => x.Action == action);
    }

    public object? GetController(string action)
    {
        return (from kvp in ControllerMapping where kvp.Value.Any(m => m.Action == action) select kvp.Key).FirstOrDefault();
    }

    public MethodInfo GetMethod(string action)
    {
        var methodMappings = ControllerMapping.Values.SelectMany(x => x).ToList();
        return methodMappings.First(x => x.Action == action).Method;
    }

    internal class MethodMapping(MethodInfo method, string action)
    {
        public MethodInfo Method { get; set; } = method;
        public string Action { get; set; } = action;
    }
}