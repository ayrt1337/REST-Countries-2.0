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

public sealed record PaginationResponse(
    [property: JsonPropertyName("currentPage")]
    int CurrentPage,
    [property: JsonPropertyName("totalPages")]
    int TotalPages,
    [property: JsonPropertyName("totalItems")]
    int TotalItems,
    [property: JsonPropertyName("pageSize")]
    int PageSize,
    [property: JsonPropertyName("count")]
    int Count
);
