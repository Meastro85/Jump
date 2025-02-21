using System.Web;

namespace Jump.Parsers;

internal static class FormParser
{

    public static async Task<object?> ParseBody(Stream body, Type paramType)
    {
        using var reader = new StreamReader(body);
        var formData = await reader.ReadToEndAsync();

        var parsedData = HttpUtility.ParseQueryString(formData);

        var constructor = paramType.GetConstructors()[0]; 

        var parameters = constructor.GetParameters();
        var constructorArgs = new object[parameters.Length];

        for (var i = 0; i < parameters.Length; i++)
        {
            var paramName = parameters[i].Name!.ToLower();
            if (parsedData[paramName] == null) continue;
            
            var value = parsedData[paramName];
            constructorArgs[i] = Convert.ChangeType(value, parameters[i].ParameterType)!;
        }

        return constructor.Invoke(constructorArgs);
    }
    
}