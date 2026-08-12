using System.Text.Json.Serialization;

namespace Countries.Application.DTOs.Responses.Countries;

public sealed record ListCountriesResponse(
    [property: JsonPropertyName("data")]
    CountriesResponse Data
);

public record CountriesResponse(
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
    long Population
);
