using System.Text.Json;

namespace Jump.Util;

internal static class JsonParser
{
    private static JsonSerializerOptions JsonSerializerOptions => new() { PropertyNameCaseInsensitive = true };
    internal static async Task<object?> ParseBody(Stream body, Type paramType)
    {
        using var reader = new StreamReader(body);
        var json = await reader.ReadToEndAsync();

        return JsonSerializer.Deserialize(json, paramType, JsonSerializerOptions);
    }
}