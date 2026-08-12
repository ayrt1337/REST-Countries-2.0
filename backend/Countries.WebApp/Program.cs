using Countries.Application.Abstractions;
using Countries.Application.Config;
using Countries.Application.Contracts.Countries;
using Countries.Infrastructure.Handler;
using Countries.Infrastructure.Transporters;
using Countries.WebApp.Bootstrap;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Countries.Application.Services.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "v1";
    config.Title = "REST Countries API";
    config.Version = "v1";
    config.Description = "API de consulta de dados de países integrada com a REST Countries.";
});

builder.Services.AddOptions<RestCountriesOptions>().Bind(builder.Configuration);

builder.Services.AddTransient<HttpDelegationHandler>();
builder.Services.AddTransient<IHttpExtensions, HttpExtensions>();

IEnumerable<Type> implementations = typeof(ICountriesServices).Assembly
    .GetTypes()
    .Where(t => t is { IsClass: true, IsAbstract: false } && t.Name.EndsWith("Services", StringComparison.Ordinal));

foreach (Type implementation in implementations)
{
    Type? interfaceType = implementation.GetInterfaces().FirstOrDefault(i => i.Name == $"I{implementation.Name}");
    if (interfaceType != null)
    {
        builder.Services.TryAddScoped(interfaceType, implementation);
    }
}

builder.Services.AddHttpClient<IRestCountriesTransport, RestCountriesTransport>(name: "RestCountries", configureClient: client =>
{
    client.BaseAddress = new Uri(builder.Configuration["RestCountriesEndpoint"]!);
})
.AddHttpMessageHandler<HttpDelegationHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();
app.MapEndpoints();

app.Run();