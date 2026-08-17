using System.Text.Json.Serialization;

namespace Countries.Application.DTOs.Responses.Countries;

public sealed record ListCountriesResponse(
    [property: JsonPropertyName("pagination")]
    PaginationResponse Pagination,
    [property: JsonPropertyName("objects")]
    IEnumerable<CountryListItemResponse> Objects
);

public sealed record RestCountriesListResponse(
    [property: JsonPropertyName("data")]
    RestCountriesDataResponse Data
);

public sealed record RestCountriesDataResponse(
    [property: JsonPropertyName("objects")]
    IEnumerable<CountryListItemResponse> Countries,
    [property: JsonPropertyName("meta")]
    RestCountriesPaginationMetaResponse Pagination
);

public sealed record CountryListItemResponse(
    [property: JsonPropertyName("names")]
    CountryNames Names,
    [property: JsonPropertyName("codes")]
    CountryCodes Codes,
    [property: JsonPropertyName("capitals")]
    IEnumerable<CountryCapital> Capitals,
    [property: JsonPropertyName("flag")]
    CountryFlag Flag,
    [property: JsonPropertyName("population")]
    long Population,
    [property: JsonPropertyName("region")]
    string Region
);

