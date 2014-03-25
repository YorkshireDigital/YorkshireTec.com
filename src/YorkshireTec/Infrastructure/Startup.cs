using Microsoft.Owin;
using YorkshireTec.Infrastructure;

[assembly: OwinStartup(typeof(Startup))]

namespace YorkshireTec.Infrastructure
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
