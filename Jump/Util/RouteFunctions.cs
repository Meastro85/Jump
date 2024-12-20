using System.Reflection;
using System.Text.RegularExpressions;
using Jump.Attributes.Actions;
using Jump.LoggingSetup;
using Jump.Providers;
using static System.Text.RegularExpressions.Regex;

namespace Jump.Util;

public static class RouteFunctions
{
    private static readonly ComponentProvider ComponentProvider = ComponentProvider.Instance;

    internal static Dictionary<string, (object Controller, MethodInfo Method)> RegisterAllRoutes(
        ICollection<Type> controllers)
    {
        var routeMappings = new Dictionary<string, (object Controller, MethodInfo Method)>();

        foreach (var controller in controllers)
        {
            var restController = ComponentProvider.GetComponent(controller);
            var routes = DiscoverRoutes(restController);

            foreach (var (route, method) in routes)
            {
                Logging.Logger.LogInformation("Registering route: " + route);
                if (!routeMappings.ContainsKey(route)) routeMappings[route] = (restController, method);
                else throw new AmbiguousMatchException($"Route {route} is ambiguous");
            }

            Logging.Logger.LogInformation("Registered REST listener: " + controller);
        }

        return routeMappings;
    }

    private static Dictionary<string, MethodInfo> DiscoverRoutes(object controller)
    {
        var routeMappings = new Dictionary<string, MethodInfo>();
        var controllerType = controller.GetType();
        foreach (var method in controllerType.GetMethods())
        {
            var routeActionAttribute = method.GetCustomAttribute<Route>();
            if (routeActionAttribute == null) continue;
            routeMappings[routeActionAttribute.Path] = method;
        }

        return routeMappings;
    }

    internal static string CreateRoutePattern(string route)
    {
        return "^" + Replace(route, @"\{(\w+)\}", "(?<$1>[^/]+)") + "$";
    }

    internal static object?[] GetRouteParameters(MethodInfo method, Match match)
    {
        var parameters = method.GetParameters();
        var args = new object?[parameters.Length];

        for (var i = 0; i < parameters.Length; i++)
        {
            var paramName = parameters[i].Name;
            if (paramName != null && match.Groups[paramName].Success)
            {
                var value = match.Groups[paramName].Value;
                args[i] = Convert.ChangeType(value, parameters[i].ParameterType);
            }
            else
            {
                args[i] = null;
            }
        }

        return args;
    }
}