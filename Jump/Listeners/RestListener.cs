using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Jump.Attributes.Actions;
using Jump.Providers;
using static System.Text.RegularExpressions.Regex;

namespace Jump.Listeners;

internal static class RestListener
{
    private static readonly ComponentProvider ComponentProvider = ComponentProvider.Instance;
    private static bool _enabled;
    private static readonly int Port = 8080;

    internal static Task RegisterRestControllers(ICollection<Type> controllers)
    {
        Console.WriteLine("Registering REST controllers");
        var routeMappings = RegisterAllRoutes(controllers);
        return StartRestListenerAsync(routeMappings);
    }

    private static Dictionary<string, (object Controller, MethodInfo Method)> RegisterAllRoutes(
        ICollection<Type> controllers)
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

    private static async Task StartRestListenerAsync(Dictionary<string, (object, MethodInfo)> routeMappings)
    {
        _enabled = true;
        using var listener = new HttpListener();
        listener.Prefixes.Add($"http://localhost:{Port}/");
        listener.Start();
        while (_enabled)
        {
            try
            {
                var context = await listener.GetContextAsync();
                var request = context.Request;
                var response = context.Response;

                var path = request.Url?.AbsolutePath.TrimEnd('/');
                if (path == null || !TryProcessRoute(path, routeMappings, out var jsonResponse))
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    await WriteResponseAsync(response, "Route not found");
                    continue;
                }

                response.StatusCode = jsonResponse!.StatusCode;
                response.ContentType = "application/json";
                await WriteResponseAsync(response, jsonResponse.ToJson());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private static async Task WriteResponseAsync(HttpListenerResponse response, string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        response.ContentLength64 = buffer.Length;

        await using var output = response.OutputStream;
        await output.WriteAsync(buffer, 0, buffer.Length);
    }

    private static bool TryProcessRoute(string input,
        Dictionary<string, (object Controller, MethodInfo Method)> routeMappings, out IJsonResponse? jsonResponse)
    {
        jsonResponse = null;

        foreach (var (route, (controller, method)) in routeMappings)
        {
            var regexPattern = CreateRoutePattern(route);
            var match = Match(input, regexPattern);

            if (!match.Success) continue;
            var parameters = GetRouteParameters(method, match);
            jsonResponse = (IJsonResponse?)method.Invoke(controller, parameters);
            return true;
        }

        return false;
    }

    private static string CreateRoutePattern(string route)
    {
        return "^" + Replace(route, @"\{(\w+)\}", "(?<$1>[^/]+)") + "$";
    }

    private static object?[] GetRouteParameters(MethodInfo method, Match match)
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