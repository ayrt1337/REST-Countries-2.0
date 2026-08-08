using Countries.Application.Abstractions;
using Countries.Application.Config;
using Countries.Infrastructure.Handler;
using Countries.Infrastructure.Transporters;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

builder.Services.Configure<RestCountriesOptions>(options => configuration.GetSection("RestCoutriesApiKey").Bind(options));

builder.Services.AddTransient<HttpDelegationHandler>();

IEnumerable<Type> implementations = typeof(HttpDelegationHandler).Assembly
    .GetTypes()
    .Where(t => t is { IsClass: true, IsAbstract: false } && t.Name.EndsWith("Service", StringComparison.Ordinal));

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
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();