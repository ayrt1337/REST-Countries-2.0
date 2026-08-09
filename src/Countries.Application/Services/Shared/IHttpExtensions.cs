using Countries.Application.DTOs.Responses;

namespace Countries.Application.Services.Shared;

public interface IHttpExtensions
{
    Task<ResultResponse<ReadOnlyMemory<byte>>> GetResultAsync(HttpResponseMessage messageResponse, CancellationToken cancellationToken);
}