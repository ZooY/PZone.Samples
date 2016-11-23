using Microsoft.Owin;
using Owin;
using PZone.Samples;


[assembly: OwinStartup(typeof(Startup))]
namespace PZone.Samples
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
