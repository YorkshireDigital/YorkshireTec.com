using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(YorkshireTec.Startup))]

namespace YorkshireTec
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
        }
    }
}
