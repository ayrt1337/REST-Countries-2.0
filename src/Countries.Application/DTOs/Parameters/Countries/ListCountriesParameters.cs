namespace Countries.Application.DTOs.Parameters.Countries;

using Microsoft.AspNetCore.Mvc;

public sealed record ListCountriesParameters(
    [FromQuery(Name = "name")]
    string? Name,
    [FromQuery(Name = "page")]
    int? Page,
    [FromQuery(Name = "region")]
    string? Region
);