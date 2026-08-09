using System.Net;
using Countries.Application.DTOs.Responses;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Countries.Application.Services.Shared;

public class HttpExtensions : IHttpExtensions
{
    public async Task<ResultResponse<ReadOnlyMemory<byte>>> GetResultAsync(HttpResponseMessage messageResponse, CancellationToken cancellationToken)
    {
        await using Stream stream = await messageResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(ConfigureAwaitOptions.None);

        return messageResponse switch
        {
            { StatusCode: HttpStatusCode.OK } => ResultResponse<ReadOnlyMemory<byte>>.CreateOk(message: "OK", data: await GetBytesAsync(stream, cancellationToken)),
            { StatusCode: HttpStatusCode.BadRequest } => ResultResponse<ReadOnlyMemory<byte>>.CreateBadRequest(message: messageResponse.ReasonPhrase ?? "Bad request", data: await GetBytesAsync(stream, cancellationToken)),
            _ => ResultResponse<ReadOnlyMemory<byte>>.CreateInternaServerlError(message: messageResponse.ReasonPhrase ?? "Unknown reasons", data: await GetBytesAsync(stream, cancellationToken))
        };
    }

    private static async Task<ReadOnlyMemory<byte>> GetBytesAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        using MemoryStream buffer = new();
        await stream.CopyToAsync(destination: buffer, cancellationToken: cancellationToken)
            .ConfigureAwait(ConfigureAwaitOptions.None);

        return buffer.GetBuffer().AsMemory(start: 0, length: checked((int) buffer.Length));
    }
}