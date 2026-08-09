using Countries.Application.DTOs.Parameters.Countries;
using Countries.Application.DTOs.Responses;
using Countries.Application.DTOs.Responses.Countries;

namespace Countries.Application.Contracts.Countries;

public interface ICountriesServices
{
    Task<ResultResponse<ListCountriesResponse>> ListCountriesAsync(ListCountriesParameters parameters, CancellationToken cancellationToken = default);
}