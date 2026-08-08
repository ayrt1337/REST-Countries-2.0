using Countries.Application.DTOs.Requests.Countries;
using Countries.Application.DTOs.Responses.Countries;

namespace Countries.Application.Contracts.Countries;

public interface ICountriesService
{
    Task<CountryResponse> ListCountriesAsync(CountriesRequest request, CancellationToken cancellationToken = default);
}