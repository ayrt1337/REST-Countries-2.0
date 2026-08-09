using System.Runtime.InteropServices;

namespace Countries.WebApp.Routing;

public static class ApiRoute
{
    public const string Slash = "/";

    public static class Countries
    {
        public const string Route = Slash + ContextRoute.Countries.Route;
        
        public static class List
        {
            public const string route = Route + Slash + ApiConstants.CommonRoute.List;
        }

        public static class GetByCode
        {
            public const string route = Route + Slash + ApiConstants.CommonRoute.GetByCode;
        }
    }
}