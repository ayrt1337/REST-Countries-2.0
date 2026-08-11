using Microsoft.AspNetCore.Mvc;

namespace Countries.Application.DTOs.Parameters.Countries;

public sealed record GetContryParameters(
    [FromRoute(Name = "code")]
    string Code
);