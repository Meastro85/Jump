namespace Jump.Parsers;

public class PropertiesFileParser
{
    private readonly Dictionary<string, string> _properties = new();

    public PropertiesFileParser(string? filePath = null)
    {
        var directory = Directory.GetCurrentDirectory();
        var filePathToUse = filePath ?? directory + "/jump.properties";
        if (!File.Exists(filePathToUse))
        {
            throw new FileNotFoundException("File not found", filePath);
        }
        
        LoadProperties(filePathToUse);
        
    }
    
    private void LoadProperties(string filePath)
    {
        foreach (var line in File.ReadAllLines(filePath))
        {
            var trimmedLine = line.Trim();
            if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith('#'))
            {
                continue;
            }

            var separatorIndex = trimmedLine.IndexOf('=');
            if (separatorIndex <= 0) continue;
            var key = trimmedLine[..separatorIndex].Trim();
            var value = trimmedLine[(separatorIndex + 1)..].Trim();

            if (!string.IsNullOrEmpty(key))
            {
                _properties[key] = value;
            }
        }
    }

    public string? Get(string key)
    {
        return _properties.TryGetValue(key, out var value) ? value : null;
    }

    public int GetInt(string key)
    {
        var value = Get(key);
        return int.TryParse(value, out var result) ? result : 0;
    }

    public bool GetBool(string key)
    {
        var value = Get(key);
        return bool.TryParse(value, out var result) && result;
    }

    public IEnumerable<string> Keys => _properties.Keys;
    
}