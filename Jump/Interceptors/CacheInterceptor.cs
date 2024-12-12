using System.Text;
using Castle.DynamicProxy;

namespace Jump.Interceptors;

public abstract class CacheInterceptor : IInterceptor
{
    public abstract void Intercept(IInvocation invocation);

    protected static string GenerateCacheKey(string key, Dictionary<string, object> parameters, string[]? paramNames)
    {
        if (parameters.Count == 0 || paramNames is null || paramNames.Length == 0) return key;

        var identifier = new StringBuilder();
        foreach (var paramName in paramNames)
        {
            if (!parameters.TryGetValue(paramName, out var paramValue))
                throw new ArgumentException($"Parameter {paramName} not found.");
            identifier.Append($"-{paramValue}");
        }

        return $"{key}{identifier}";
    }

    protected static Dictionary<string, object> GetParameters(IInvocation invocation)
    {
        var parameters = invocation.Arguments;
        var parameterInfo = invocation.Method.GetParameters();
        var parametersDictionary = new Dictionary<string, object>();

        for (var i = 0; i < invocation.Arguments.Length; i++)
        {
            var parameter = parameterInfo[i];
            var parameterName = parameter.Name;
            var parameterValue = parameters[i];

            // Add the parameter name and value to the dictionary
            parametersDictionary.Add(parameterName!, parameterValue);
        }

        return parametersDictionary;
    }
}