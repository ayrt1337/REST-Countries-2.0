using System.Text.Json;
using Countries.Application.Abstractions;
using Countries.Application.DTOs.Responses;
using Countries.Application.Serialization;
using Countries.Application.Services.Shared;

namespace Countries.Infrastructure.Transporters;

public sealed class RestCountriesTransport(HttpClient httpClient, IHttpExtensions httpExtensions)
    : IRestCountriesTransport
{
    private readonly JsonSerializerOptions _defaultOptions = new(DefaultOptions.Serializer);

    public async Task<ResultResponse<TData>> SendAsync<TData>(string address, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(address);

        using var response = await httpClient
            .GetAsync(address, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(ConfigureAwaitOptions.None);

        var result = await httpExtensions.GetResultAsync(response, cancellationToken);

        if (!response.IsSuccessStatusCode)
            return ResultResponse<TData>.Create(response.StatusCode, result.Message);

        if (result.Data.Span.IsEmpty)
            return ResultResponse<TData>.CreateBadGateway("Conteúdo não retornado.");

        try
        {
            var data = JsonSerializer.Deserialize<TData>(result.Data.Span, _defaultOptions);

            return data is null
                ? ResultResponse<TData>.CreateBadGateway("Conteúdo não retornado.")
                : ResultResponse<TData>.CreateOk(result.Message, data);
        }
        catch (JsonException)
        {
            return ResultResponse<TData>.CreateBadGateway("Conteúdo retornado não compatível com contrato.");
        }
    }
}