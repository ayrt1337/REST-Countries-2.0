namespace Countries.Application.Resources;

public static class ResourcesNames
{
    private const string Slash = "/";
    private const string ApiVersion = "v5";
    public static class Countries
    {
        public const string Route = Slash + "countries" + Slash + ApiVersion;
    }
}