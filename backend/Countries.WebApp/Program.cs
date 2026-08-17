using Countries.Application.Abstractions;
using Countries.Application.Config;
using Countries.Application.Contracts.Countries;
using Countries.Infrastructure.Handler;
using Countries.Infrastructure.Transporters;
using Countries.WebApp.Bootstrap;
using Countries.Application.Services.Shared;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "FrontendCorsPolicy", policy =>
    {
        policy.WithOrigins(builder.Configuration["Cors:FrontEnd"] ?? "http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "v1";
    config.Title = "REST Countries API";
    config.Version = "v1";
    config.Description = "Country data query API integrated with REST Countries.";
});

builder.Services.AddOptions<RestCountriesOptions>().Bind(builder.Configuration);

builder.Services.AddMemoryCache();

builder.Services.AddTransient<HttpDelegationHandler>();
builder.Services.AddTransient<HttpExtensions>();

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
    client.Timeout = TimeSpan.FromSeconds(10);
})
.AddHttpMessageHandler<HttpDelegationHandler>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseCors("FrontendCorsPolicy");

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapEndpoints();

app.Run();
