using Microsoft.Owin;
using YorkshireTec.Api.Infrastructure;

[assembly: OwinStartup(typeof(Startup))]

namespace YorkshireTec.Api.Infrastructure
{
    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
        }
    }
}
