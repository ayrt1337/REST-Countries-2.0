using Countries.Application.DTOs.Responses;
using Countries.Application.DTOs.Responses.Countries;

namespace Countries.Application.Abstractions;

public interface IRestCountriesTransport
{
    Task<ResultResponse<TData>> SendAsync<TData>(string address, CancellationToken cancellationToken);
}