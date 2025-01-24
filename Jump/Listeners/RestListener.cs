using System.Net;
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

    private static async Task StartRestListenerAsync(
        IDictionary<string, RouteMapping> routeMappings)
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
                var method = request.HttpMethod;

                var path = request.Url?.AbsolutePath.TrimEnd('/');
                if (path == null || !TryProcessRoute(path, method, routeMappings, out var r))
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    await WriteResponseAsync(response, "Route not found");
                    continue;
                }

                response.StatusCode = r!.StatusCode;
                response.ContentType = DetermineContentType(r.GetType());
                await WriteResponseAsync(response, r.GetResponse());
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

    private static string DetermineContentType(Type responseType)
    {
        return responseType switch
        {
            not null when responseType == typeof(string) || responseType == typeof(Response) => "text/plain",
            not null when responseType == typeof(JsonResponse<>) => "application/json",
            _ => "application/json"
        };
    }

    private static bool TryProcessRoute(string input, string action,
        IDictionary<string, RouteMapping> routeMappings,
        out IResponse? response)
    {
        response = null;

        try
        {
            var mapping = routeMappings.First(mapping => Match(input, mapping.Key).Success);
            if (!mapping.Value.ActionExists(action))
            {
                response = new Response((int)HttpStatusCode.MethodNotAllowed, "Method not allowed");
                return true;
            }

            var method = mapping.Value.GetMethod(action);
            var controller = mapping.Value.GetController(action);

            var parameters = RouteFunctions.GetRouteParameters(method, Match(input, mapping.Key));
            response = (IResponse?)method.Invoke(controller, parameters);
            return true;
        }
        catch (InvalidOperationException ex)
        {
            Logging.Logger.LogError("Route not found", ex);
            return false;
        }
    }
}