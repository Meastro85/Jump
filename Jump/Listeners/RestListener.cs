using System.Net;
using System.Reflection;
using System.Text;
using Jump.Http_response;
using Jump.LoggingSetup;
using Jump.Util;
using static System.Text.RegularExpressions.Regex;

namespace Jump.Listeners;

internal static class RestListener
{
    private static bool _enabled;
    private static readonly int Port = 8080;

    internal static Task RegisterRestControllers(ICollection<Type> controllers)
    {
        var routeMappings = RouteFunctions.RegisterAllRoutes(controllers);
        return StartRestListenerAsync(routeMappings);
    }

    private static async Task StartRestListenerAsync(Dictionary<string, (object, MethodInfo)> routeMappings)
    {
        _enabled = true;
        using var listener = new HttpListener();
        listener.Prefixes.Add($"http://localhost:{Port}/");
        listener.Start();
        while (_enabled)
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
                Logging.Logger.LogError("Error processing request", ex);
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
            var regexPattern = RouteFunctions.CreateRoutePattern(route);
            var match = Match(input, regexPattern);

            if (!match.Success) continue;
            var parameters = RouteFunctions.GetRouteParameters(method, match);
            jsonResponse = (IJsonResponse?)method.Invoke(controller, parameters);
            return true;
        }

        return false;
    }
}