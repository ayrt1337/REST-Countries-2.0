using Countries.WebApp.Endpoints.Countries;

namespace Countries.WebApp.Bootstrap;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapCountriesEndpoints();

        return routes;
    }
}