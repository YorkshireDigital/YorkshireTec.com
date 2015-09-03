using System;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(YorkshireDigital.Hangfire.Startup))]

namespace YorkshireDigital.Hangfire
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseErrorPage();
            app.UseWelcomePage("/");

            GlobalConfiguration.Configuration.UseSqlServerStorage("Database", new SqlServerStorageOptions { QueuePollInterval = TimeSpan.FromSeconds(1) });

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            SetUpSystemJobs();
        }

        public void SetUpSystemJobs()
        {
            // Set up recurring system tasks here
        }
    }
}
