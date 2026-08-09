using Countries.WebApp.Endpoints.Countries.Handlers;
using Countries.WebApp.Routing;

namespace Countries.WebApp.Endpoints.Countries;

using static ApiRoute.Countries;

public static class CountriesEnpoints
{
    public static IEndpointRouteBuilder MapCountriesEndpoints(this IEndpointRouteBuilder routes)
    {
        IEndpointRouteBuilder group = routes.MapGroup(prefix: Route).WithTags("Countries");

        //group.MapGet(pattern: GetByCode.route, handler: CountriesHandlers.GetListCountriesAsync)
        //     .WithName(nameof(CountriesHandlers.GetListCountriesAsync));

        group.MapGet(pattern: List.route, handler: CountriesHandlers.GetListCountriesAsync)
             .WithName(nameof(CountriesHandlers.GetListCountriesAsync));

        return routes;
    }
}