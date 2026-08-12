using System.Text.Json;

namespace Countries.Application.Serialization;

public static class DefaultOptions
{
    public static JsonSerializerOptions Serializer = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        MaxDepth = 32,
    };
}