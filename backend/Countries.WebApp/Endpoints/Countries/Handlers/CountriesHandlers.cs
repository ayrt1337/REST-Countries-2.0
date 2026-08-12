using Countries.Application.Contracts.Countries;
using Countries.Application.DTOs.Parameters.Countries;
using Countries.Application.DTOs.Responses;
using Countries.Application.DTOs.Responses.Countries;
using Microsoft.AspNetCore.Mvc;

namespace Countries.WebApp.Endpoints.Countries.Handlers;

public static class CountriesHandlers
{
    public static async Task<ResultResponse<ListCountriesResponse>> GetListCountriesAsync([AsParameters] ListCountriesParameters parameters, [FromServices] ICountriesServices services, CancellationToken cancellationToken = default)
    {
        ResultResponse<ListCountriesResponse> response =
            await services.ListCountriesAsync(parameters: parameters, cancellationToken: cancellationToken).ConfigureAwait(ConfigureAwaitOptions.None);
        
        return response;
    }

    public static async Task<ResultResponse<CountryResponse>> GetCountryAsync([AsParameters] GetContryParameters parameters, [FromServices] ICountriesServices services, CancellationToken cancellationToken = default)
    {
        ResultResponse<CountryResponse> response =
            await services.GetCountryAsync(parameters: parameters, cancellationToken: cancellationToken).ConfigureAwait(ConfigureAwaitOptions.None);

        return response;
    }
} 