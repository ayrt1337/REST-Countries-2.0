using System.Text.Json.Serialization;

namespace Countries.Application.DTOs.Responses.Countries;

public sealed record RestCountriesPaginationMetaResponse(
    [property: JsonPropertyName("total")]
    int Total,
    [property: JsonPropertyName("count")]
    int Count,
    [property: JsonPropertyName("limit")]
    int Limit,
    [property: JsonPropertyName("offset")]
    int Offset
);