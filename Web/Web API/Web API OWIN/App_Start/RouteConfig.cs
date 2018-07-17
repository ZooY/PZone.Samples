using System.Web.Http;


namespace PZone.Samples
{
    public class RouteConfig
    {
        public static void RegisterRoutes(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}