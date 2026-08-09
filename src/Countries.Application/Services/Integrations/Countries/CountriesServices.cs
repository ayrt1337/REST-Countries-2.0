using Countries.Application.Abstractions;
using Countries.Application.Contracts.Countries;
using Countries.Application.DTOs.Parameters.Countries;
using Countries.Application.DTOs.Responses;
using Countries.Application.DTOs.Responses.Countries;

namespace Countries.Application.Services.Integrations.Countries;

internal sealed class CountriesServices(IRestCountriesTransport transport) : ICountriesServices
{
    public async Task<ResultResponse<ListCountriesResponse>> ListCountriesAsync(ListCountriesParameters parameters, CancellationToken cancellationToken = default)
    {
        string query = "?limit=50&response_fields=names.common,codes.cioc,capitals,flag.url_svg,population";

        if (parameters.Page > 1)
            query += $"&offset={parameters.Page * 50}";

        if (!string.IsNullOrWhiteSpace(parameters.Region))
            query += $"&region={parameters.Region}";

        if (!string.IsNullOrWhiteSpace(parameters.Name))
            query += $"&q={parameters.Name}";

        string address = Resources.ResourcesNames.Countries.Route + query;

        return await transport.SendAsync<ListCountriesResponse>(address: address, cancellationToken: cancellationToken).ConfigureAwait(ConfigureAwaitOptions.None);
    }
}