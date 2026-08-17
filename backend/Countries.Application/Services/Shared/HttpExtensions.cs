using System.Net;
using Countries.Application.DTOs.Responses;

namespace Countries.Application.Services.Shared;

public class HttpExtensions
{
    public async Task<ResultResponse<ReadOnlyMemory<byte>>> GetResultAsync(HttpResponseMessage messageResponse, CancellationToken cancellationToken)
    {
        await using var stream = await messageResponse.Content.ReadAsStreamAsync(cancellationToken)
            .ConfigureAwait(ConfigureAwaitOptions.None);

        return messageResponse switch
        {
            { StatusCode: HttpStatusCode.OK } => ResultResponse<ReadOnlyMemory<byte>>.CreateOk("OK",
                await GetBytesAsync(stream, cancellationToken)),
            { StatusCode: HttpStatusCode.BadRequest } => ResultResponse<ReadOnlyMemory<byte>>.CreateBadRequest(
                messageResponse.ReasonPhrase ?? "Bad request", await GetBytesAsync(stream, cancellationToken)),
            _ => ResultResponse<ReadOnlyMemory<byte>>.CreateInternaServerlError(
                messageResponse.ReasonPhrase ?? "Unknown reasons", await GetBytesAsync(stream, cancellationToken))
        };
    }

    private static async Task<ReadOnlyMemory<byte>> GetBytesAsync(Stream stream,
        CancellationToken cancellationToken = default)
    {
        using MemoryStream buffer = new();
        await stream.CopyToAsync(buffer, cancellationToken)
            .ConfigureAwait(ConfigureAwaitOptions.None);

        return buffer.GetBuffer().AsMemory(0, checked((int)buffer.Length));
    }
}