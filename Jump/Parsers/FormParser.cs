using System.Reflection;
using System.Web;

namespace Jump.Parsers;

internal static class FormParser
{

    public static async Task<object?> ParseBody(Stream body, Type paramType)
    {
        using var reader = new StreamReader(body);
        var formData = await reader.ReadToEndAsync();

        var parsedData = HttpUtility.ParseQueryString(formData);

        var instance = Activator.CreateInstance(paramType);
        
        var properties = paramType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var fieldName = property.Name.ToLower();
            if (parsedData[fieldName] == null) continue;

            var value = Convert.ChangeType(parsedData[fieldName], property.PropertyType);
            property.SetValue(instance, value);
        }

        return instance;
    }
    
}