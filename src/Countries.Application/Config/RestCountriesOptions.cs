namespace Countries.Application.Config;

public class RestCountriesOptions(string restCountriesApiKey)
{
    public string RestCountriesApiKey { get; init; } = restCountriesApiKey;
}