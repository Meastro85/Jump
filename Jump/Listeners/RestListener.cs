using System.Reflection;
using System.Text.RegularExpressions;
using Jump.Attributes.Actions;
using Jump.Exceptions;
using Jump.Providers;
using static System.Text.RegularExpressions.Regex;

namespace Jump.Listeners;

internal static class RestListener
{
    
    private static readonly ComponentProvider ComponentProvider = ComponentProvider.Instance;

    internal static Task RegisterRestControllers(ICollection<Type> controllers)
    {
        Console.WriteLine("Registering REST controllers");
        var routeMappings = RegisterAllRoutes(controllers);
        return StartRestListenerAsync(routeMappings);
    }
    
    private static Dictionary<string, (object Controller, MethodInfo Method)> RegisterAllRoutes(ICollection<Type> controllers)
    {
        var routeMappings = new Dictionary<string, (object Controller, MethodInfo Method)>();
        
        foreach (var controller in controllers)
        {
            var restController = ComponentProvider.GetComponent(controller);
            var routes = DiscoverRoutes(restController);

            foreach (var (route, method) in routes)
            {
                if (!routeMappings.ContainsKey(route)) routeMappings[route] = (restController, method);
                else throw new AmbiguousMatchException($"Route {route} is ambiguous");
            }
        }
        return routeMappings;
    }
    
    private static Dictionary<string, MethodInfo> DiscoverRoutes(object controller)
    {
        var routeMappings = new Dictionary<string , MethodInfo>();
        var controllerType = controller.GetType();
        foreach (var method in controllerType.GetMethods())
        {
            var routeActionAttribute = method.GetCustomAttribute<Route>();
            if (routeActionAttribute == null) continue;
            routeMappings[routeActionAttribute.Path] = method;
        }
        return routeMappings;
    }
    
    private static async Task StartRestListenerAsync(Dictionary<string, (object, MethodInfo)> routeMappings)
    {
            while (true)
            {
                var input = await Task.Run(Console.ReadLine);
                if(string.IsNullOrWhiteSpace(input)) continue;
                if(!TryProcessInput(input, routeMappings)) Console.WriteLine("No matching routes found");
            }
    }

    private static bool TryProcessInput(string input,
        Dictionary<string, (object Controller, MethodInfo Method)> routeMappings)
    {
        foreach (var (route, (controller, method)) in routeMappings)
        {
            var match = Match(input, CreateRoutePattern(route));
            if (!match.Success) continue;
            InvokeAction(controller, method, match);
            return true;
        }

        return false;
    }
    
    private static void InvokeAction(object controller, MethodInfo method, Match match)
    {
        var parameters = method.GetParameters();
        var args = new object[parameters.Length];

        for (var i = 0; i < parameters.Length; i++)
        {
            var paramName = parameters[i].Name;
            if (paramName == null) continue;
            args[i] = Convert.ChangeType(match.Groups[paramName].Value, parameters[i].ParameterType);
        }

        method.Invoke(controller, args);
    }
    
    private static string CreateRoutePattern(string route)
    {
        return "^" + Replace(route, @"\{(\w+)\}", "(?<$1>[^/]+)") + "$";
    }
    
}