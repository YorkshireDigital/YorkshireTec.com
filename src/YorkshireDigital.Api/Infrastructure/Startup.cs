using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(YorkshireDigital.Api.Startup))]

namespace YorkshireDigital.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
        }
    }
}
