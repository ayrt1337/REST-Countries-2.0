using Countries.Application.DTOs.Responses;

namespace Countries.Application.Abstractions;

public interface IRestCountriesTransport
{
    Task<ResultResponse<TData>> SendAsync<TData>(string address, CancellationToken cancellationToken);
}