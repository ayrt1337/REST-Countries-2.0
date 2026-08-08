using Countries.Application.Contracts.Countries;
using Countries.Application.DTOs.Requests.Countries;
using Countries.Application.DTOs.Responses.Countries;

namespace Countries.Application.Services.Integrations.Countries;

internal sealed class CountriesServices : ICountriesService
{
    public Task<CountryResponse> ListCountriesAsync(CountriesRequest request, CancellationToken cancellationToken = default)
    {

    }
}