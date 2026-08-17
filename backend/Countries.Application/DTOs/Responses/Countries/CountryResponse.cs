using System.Text.Json.Serialization;

namespace Countries.Application.DTOs.Responses.Countries;

public sealed record CountryResponse(
    [property: JsonPropertyName("data")]
    CountryDetailDataResponse Data
);

public sealed record CountryDetailDataResponse(
    [property: JsonPropertyName("objects")]
    IEnumerable<CountryItemResponse> Countries
);

public sealed record CountryItemResponse(
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
    IEnumerable<BorderCountryResponse> Borders,
    [property: JsonPropertyName("population")]
    long Population
);

public sealed record BorderCountryResponse(
    [property: JsonPropertyName("commonName")]
    string CommonName,
    [property: JsonPropertyName("alpha3")]
    string Alpha3
);

public sealed record RestCountryResponse(
    [property: JsonPropertyName("data")]
    RestCountryDetailDataResponse Data
);

public sealed record RestCountryDetailDataResponse(
    [property: JsonPropertyName("objects")]
    IEnumerable<RestCountryItemResponse> Countries
);

public sealed record RestCountryItemResponse(
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
    IEnumerable<string>? Borders,
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
    [property: JsonPropertyName("alpha_3")]
    string Alpha3
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