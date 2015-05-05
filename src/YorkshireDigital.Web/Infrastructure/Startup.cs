using Microsoft.Owin;
using Owin;
using Hangfire;

[assembly: OwinStartup(typeof(YorkshireDigital.Web.Startup))]

namespace YorkshireDigital.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("Database");

            app.UseHangfireDashboard();
            app.UseHangfireServer();
            app.UseNancy();
        }
    }
}
