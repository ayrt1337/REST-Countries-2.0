using System.Net.Http.Headers;
using Countries.Application.Config;
using Microsoft.Extensions.Options;

namespace Countries.Infrastructure.Handler;

public class HttpDelegationHandler(IOptions<RestCountriesOptions> options) : DelegatingHandler
{
    private readonly RestCountriesOptions _options = options.Value;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("Authorization", $"Bearer {_options.RestCountriesApiKey}");
        return base.SendAsync(request, cancellationToken);
    }
}