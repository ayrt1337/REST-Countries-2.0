using System.Net;
using Microsoft.Extensions.Caching.Memory;
using Countries.Application.Abstractions;
using Countries.Application.Contracts.Countries;
using Countries.Application.DTOs.Parameters.Countries;
using Countries.Application.DTOs.Responses;
using Countries.Application.DTOs.Responses.Countries;
using Countries.Application.Services.Shared;

namespace Countries.Application.Services.Integrations.Countries;

internal sealed class CountriesServices(IRestCountriesTransport transport, IMemoryCache cache) : ICountriesServices
{
    private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromDays(7);

    public async Task<ResultResponse<ListCountriesResponse>> ListCountriesAsync(ListCountriesParameters parameters, CancellationToken cancellationToken = default)
    {
        string cacheKey = $"countries_list_{parameters.Region}_{parameters.Name}_{parameters.Page ?? 1}";

        return (await cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = DefaultCacheDuration;

            string address = Resources.ResourcesNames.Countries.Route + "?classification.iso_status=official&limit=50&response_fields=names.common,names.official,codes.alpha_3,capitals,flag.url_svg,population,region";

            if (parameters.Page > 1)
                address += $"&offset={(parameters.Page.Value - 1) * 50}";

            if (!string.IsNullOrWhiteSpace(parameters.Region))
                address += $"&region={Uri.EscapeDataString(parameters.Region.Trim())}";

            if (!string.IsNullOrWhiteSpace(parameters.Name))
                address += $"&names.common={Uri.EscapeDataString(parameters.Name.Trim())}";

            ResultResponse<RestCountriesListResponse> response = await transport.SendAsync<RestCountriesListResponse>(address: address, cancellationToken: cancellationToken).ConfigureAwait(ConfigureAwaitOptions.None);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1);
                return ResultResponse<ListCountriesResponse>.Create(statusCode: response.StatusCode, message: response.Message);
            }

            if (response.Data?.Data == null || !response.Data.Data.Countries.Any())
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1);
                return ResultResponse<ListCountriesResponse>.CreateNotFound(message: "Country not found.");
            }

            var formattedPagination = PaginationFormatter.Format(response.Data.Data.Pagination);

            var listResponse = new ListCountriesResponse(
                Pagination: formattedPagination,
                Objects: response.Data.Data.Countries
            );

            return ResultResponse<ListCountriesResponse>.CreateOk(message: response.Message, data: listResponse);
        }).ConfigureAwait(ConfigureAwaitOptions.None))!;
    }

    public async Task<ResultResponse<CountryResponse>> GetCountryAsync(GetContryParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(parameters.Code))
            return ResultResponse<CountryResponse>.CreateBadRequest(message: "Country identification code is required.");

        string cacheKey = $"country_detail_{parameters.Code.ToUpperInvariant().Trim()}";

        return (await cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = DefaultCacheDuration;

            string address = Resources.ResourcesNames.Countries.Route + "/codes.alpha_3/" + Uri.EscapeDataString(parameters.Code.Trim());

            ResultResponse<RestCountryResponse> response = await transport.SendAsync<RestCountryResponse>(address: address, cancellationToken: cancellationToken).ConfigureAwait(ConfigureAwaitOptions.None);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1);
                return ResultResponse<CountryResponse>.Create(statusCode: response.StatusCode, message: response.Message);
            }

            RestCountryItemResponse? mainCountry = response.Data?.Data?.Countries?.FirstOrDefault();

            if (mainCountry == null)
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1);
                return ResultResponse<CountryResponse>.CreateNotFound(message: "Country not found.");
            }

            List<BorderCountryResponse> borderCountryResponses = [];

            if (mainCountry.Borders != null && mainCountry.Borders.Any())
            {
                var borderTasks = mainCountry.Borders.Select(async borderCode =>
                {
                    string borderCacheKey = $"border_name_{borderCode.ToUpperInvariant()}";
                    return await cache.GetOrCreateAsync(borderCacheKey, async borderEntry =>
                    {
                        borderEntry.AbsoluteExpirationRelativeToNow = DefaultCacheDuration;

                        string borderAddress = Resources.ResourcesNames.Countries.Route + "/codes.alpha_3/" + borderCode + "?response_fields=names.common,codes.alpha_3";
                        ResultResponse<RestCountryResponse> borderResponse = await transport.SendAsync<RestCountryResponse>(address: borderAddress, cancellationToken: cancellationToken).ConfigureAwait(ConfigureAwaitOptions.None);

                        if (borderResponse.StatusCode == HttpStatusCode.OK && borderResponse.Data?.Data?.Countries?.FirstOrDefault() is { } borderCountry)
                        {
                            return new BorderCountryResponse(
                                CommonName: borderCountry.Names?.CommonName ?? borderCode,
                                Alpha3: borderCountry.Codes?.Alpha3 ?? borderCode
                            );
                        }

                        borderEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1);
                        return new BorderCountryResponse(CommonName: borderCode, Alpha3: borderCode);
                    }).ConfigureAwait(ConfigureAwaitOptions.None);
                });

                borderCountryResponses = (await Task.WhenAll(borderTasks).ConfigureAwait(ConfigureAwaitOptions.None))
                    .Where(b => b != null)!
                    .ToList()!;
            }

            var countryItem = new CountryItemResponse(
                Names: mainCountry.Names,
                Codes: mainCountry.Codes,
                Capitals: mainCountry.Capitals,
                Flag: mainCountry.Flag,
                Currencies: mainCountry.Currencies,
                Region: mainCountry.Region,
                SubRegion: mainCountry.SubRegion,
                Continents: mainCountry.Continents,
                Borders: borderCountryResponses,
                Population: mainCountry.Population
            );

            var countryResponse = new CountryResponse(
                Data: new CountryDetailDataResponse(
                    Countries: [countryItem]
                )
            );

            return ResultResponse<CountryResponse>.CreateOk(message: response.Message, data: countryResponse);
        }).ConfigureAwait(ConfigureAwaitOptions.None))!;
    }
}