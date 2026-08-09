namespace Countries.WebApp.Routing;

public static class ApiConstants
{
    public static class Method
    {
        public const string GetByCode = "{code}";
        public const string List = "list";
    }

    public static class CommonRoute
    {
        public const string GetByCode = Method.GetByCode;
        public const string List = Method.List;
    }
}