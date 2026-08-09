using System.Text.Json;
using System.Xml;
using Countries.Application.Abstractions;
using Countries.Application.DTOs.Responses;
using Countries.Application.DTOs.Responses.Countries;
using Countries.Application.Services.Shared;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Countries.Infrastructure.Transporters;

public sealed class RestCountriesTransport(HttpClient httpClient, IHttpExtensions httpExtensions) : IRestCountriesTransport
{
    private readonly JsonSerializerOptions _defaultOptions = new (options: Application.Serialization.DefaultOptions.Serializer);

    public async Task<ResultResponse<TData>> SendAsync<TData>(string address, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(address);

        using HttpResponseMessage response = await httpClient.GetAsync(requestUri: address, completionOption: HttpCompletionOption.ResponseHeadersRead, cancellationToken: cancellationToken).ConfigureAwait(ConfigureAwaitOptions.None);

        ResultResponse<ReadOnlyMemory<byte>> result = await httpExtensions.GetResultAsync(response, cancellationToken);

        if (!response.IsSuccessStatusCode)
            return ResultResponse<TData>.Create(statusCode: response.StatusCode, message: result.Message);

        if (result.Data.Span.IsEmpty)
            return ResultResponse<TData>.CreateBadGateway(message: "Conteúdo não retornado.");

        try
        {
            TData? data = JsonSerializer.Deserialize<TData>(result.Data.Span, _defaultOptions);

            return data is null
                ? ResultResponse<TData>.CreateBadGateway(message: "Conteúdo não retornado.")
                : ResultResponse<TData>.CreateOk(message: result.Message, data: data);
        }
        catch (JsonException)
        {
            return ResultResponse<TData>.CreateBadGateway(message: "Conteúdo retornado não compatível com contrato.");
        }
    }
}