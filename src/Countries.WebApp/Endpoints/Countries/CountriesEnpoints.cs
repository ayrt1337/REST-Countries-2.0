using Countries.WebApp.Endpoints.Countries.Handlers;

namespace Countries.WebApp.Endpoints.Countries;

public static class CountriesEnpoints
{
    public static IEndpointRouteBuilder MapCountriesEndpoints(this IEndpointRouteBuilder routes)
    {
        IEndpointRouteBuilder group = routes.MapGroup("").WithTags("Countries");

        group.MapGet(pattern: "Get", handler: CountriesHandlers.GetListCountriesAsync)
            .WithName(nameof(CountriesHandlers.GetListCountriesAsync));

        return routes;
    }
}