using System.Xml.Serialization;

namespace Jump.Parsers;

internal static class XmlParser
{
    
    internal static async Task<object?> ParseBody(Stream body, Type paramType)
    {
        using var reader = new StreamReader(body);
        var xmlContent = await reader.ReadToEndAsync();
        var xmlSerializer = new XmlSerializer(paramType);

        using var stringReader = new StringReader(xmlContent);
        return xmlSerializer.Deserialize(stringReader);
    }
    
}