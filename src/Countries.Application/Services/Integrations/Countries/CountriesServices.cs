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
        string address = Resources.ResourcesNames.Countries.Route + "?classification.iso_status=official&limit=50&response_fields=&names.common,names.official,codes.alpha_3,capitals,flag.url_svg,population";

        if (parameters.Page > 1)
            address += $"&offset={parameters.Page * 50}";

        if (!string.IsNullOrWhiteSpace(parameters.Region))
            address += $"&region={parameters.Region}";

        if (!string.IsNullOrWhiteSpace(parameters.Name))
            address += $"&names.common={parameters.Name}";

        return await transport.SendAsync<ListCountriesResponse>(address: address, cancellationToken: cancellationToken).ConfigureAwait(ConfigureAwaitOptions.None);
    }

    public async Task<ResultResponse<CountryResponse>> GetCountryAsync(GetContryParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(parameters.Code))
            return ResultResponse<CountryResponse>.CreateBadRequest(message: "Código de identificação do país é obrigatório.");

        string address = Resources.ResourcesNames.Countries.Route + "/codes.alpha_3/" + parameters.Code;

        return await transport.SendAsync<CountryResponse>(address: address, cancellationToken: cancellationToken).ConfigureAwait(ConfigureAwaitOptions.None);
    }
}