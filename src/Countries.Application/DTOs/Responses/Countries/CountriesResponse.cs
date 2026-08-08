using System.Text.Json.Serialization;

namespace Countries.Application.DTOs.Responses.Countries;

public sealed record CountriesResponse(
    [property: JsonPropertyName("data")]
    CountriesDataResponse Data
);

public sealed record CountriesDataResponse(
    [property: JsonPropertyName("objects")]
    IEnumerable<CountryResponse> Countries,
    [property: JsonPropertyName("meta")]
    CountriesPaginationResponse Pagination
);

public sealed record CountryResponse(
    [property: JsonPropertyName("names")]
    CountryNames Names,
    [property: JsonPropertyName("codes")]
    CountryCodes Codes,
    [property: JsonPropertyName("capitals")]
    IEnumerable<CountryCapital> Capitals,
    [property: JsonPropertyName("flag")]
    CountryFlag Flag,
    [property: JsonPropertyName("currencies")]
    IEnumerable<RestCountryCurrencies> Currencies,
    [property: JsonPropertyName("region")]
    string Region,
    [property: JsonPropertyName("subregion")]
    string SubRegion,
    [property: JsonPropertyName("continents")]
    IEnumerable<string> Continents,
    [property: JsonPropertyName("borders")]
    IEnumerable<string> Borders,
    [property: JsonPropertyName("population")]
    long Population
);

public sealed record CountryNames(
    [property: JsonPropertyName("common")]
    string CommonName,
    [property: JsonPropertyName("official")]
    string OfficialName
);

public sealed record CountryCodes(
    [property: JsonPropertyName("cioc")]
    string CiocCode
);

public sealed record CountryCapital(
    [property: JsonPropertyName("name")]
    string CapitalName
);

public sealed record CountryFlag(
    [property: JsonPropertyName("url_svg")]
    string SvgFlag
);

public sealed record RestCountryCurrencies(
    [property: JsonPropertyName("name")]
    string CurrencyName,
    [property: JsonPropertyName("symbol")]
    string CurrencySymbol
);