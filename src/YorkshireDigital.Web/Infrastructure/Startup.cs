using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(YorkshireDigital.Web.Startup))]

namespace YorkshireDigital.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
        }
    }
}
