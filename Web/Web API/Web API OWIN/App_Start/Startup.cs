using System.Web.Http;
using Microsoft.Owin;
using Owin;
using PZone.Samples;


[assembly: OwinStartup(typeof(Startup))]


namespace PZone.Samples
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            RouteConfig.RegisterRoutes(config);
            app.UseWebApi(config);
        }
    }
}