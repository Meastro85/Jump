using System.Reflection;
using Jump.Attributes.Actions;
using static System.Text.RegularExpressions.Regex;

namespace Jump.Listeners;

public class RestListener
{
    private static Dictionary<string, MethodInfo> RegisterRestController(object controller)
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

    private static async Task StartRestListenerAsync(object controller, Dictionary<string, MethodInfo> routeMappings)
    {
        await Task.Run(() =>
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (input == "exit") break;
                foreach (var route in routeMappings.Keys)
                {
                    var routePattern = CreateRoutePattern(route);
                    if (input == null) continue;

                    var match = Match(input, routePattern);
                    if (!match.Success) continue;

                    var method = routeMappings[route];
                    var parameters = method.GetParameters();
                    var args = new object[parameters.Length];

                    for (var i = 0; i < parameters.Length; i++)
                    {
                        var paramName = parameters[i].Name;
                        if (paramName != null)
                            args[i] = Convert.ChangeType(match.Groups[paramName].Value, parameters[i].ParameterType);
                    }

                    method.Invoke(controller, args);
                    break;
                }
            }
        });
    }

    public static IEnumerable<Task> RegisterRestControllers(ICollection<Type> controllers)
    {
        Console.WriteLine("Registering REST controllers");
        foreach (var controller in controllers)
        {
            var constructor = controller.GetConstructors()[0];
            var restController = constructor.Invoke(null);
            var routeMappings = RegisterRestController(restController);
            yield return StartRestListenerAsync(restController, routeMappings);
        }
    }
    
    private static string CreateRoutePattern(string route)
    {
        return "^" + Replace(route, @"\{(\w+)\}", "(?<$1>[^/]+)") + "$";
    }
    
}