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

    internal static IDictionary<string, RouteMapping> RegisterAllRoutes(ICollection<Type> controllers)
    {
        var routeMappings = new Dictionary<string, RouteMapping>();

        foreach (var controller in controllers)
        {
            var restController = ComponentProvider.GetComponent(controller);
            var routes = DiscoverRoutes(restController);

            foreach (var (route, method) in routes)
            {
                Logging.Logger.LogInformation("Registering route: " + route);

                var action = method.GetCustomAttributes<Route>().First().HttpAction;
                var patternedRoute = CreateRoutePattern(route);

                if (!routeMappings.ContainsKey(patternedRoute))
                {
                    var routeMapping = new RouteMapping();
                    routeMapping.ControllerMapping.Add(restController,
                        [new RouteMapping.MethodMapping(method, action)]);
                    routeMappings[patternedRoute] = routeMapping;
                }
                else if (routeMappings.TryGetValue(patternedRoute, out var mapping) &&
                         !mapping.ActionExists(action))
                {
                    var methodMapping = new RouteMapping.MethodMapping(method, action);
                    if (mapping.ControllerMapping.TryGetValue(restController, out var controllerMappings))
                    {
                        controllerMappings.Add(methodMapping);
                    }
                    else
                    {
                        mapping.ControllerMapping[restController] = new List<RouteMapping.MethodMapping> { methodMapping };
                    }
                }
                else
                {
                    throw new AmbiguousMatchException($"Route {route} is ambiguous");
                }
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