using System;
using System.Runtime;
using Hangfire.Logging;
using Hangfire.Logging.LogProviders;
using Microsoft.Owin.Hosting;
using Topshelf;
using Serilog;

namespace YorkshireDigital.Hangfire
{
    class Program
    {
        private const string Endpoint = "http://localhost:12347";

        static void Main()
        {
            LogProvider.SetCurrentLogProvider(new ColouredConsoleLogProvider());

            HostFactory.Run(x =>
            {
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.RollingFile(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\hangfire-{Date}.log")
                    .WriteTo.ColoredConsole()
                    .MinimumLevel.Information()
                    .CreateLogger();

                x.Service<Application>(s =>
                {
                    s.ConstructUsing(name => new Application());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.UseSerilog();

                x.SetDescription("YorkshireDigital Hangfire Service");
                x.SetDisplayName("YorkshireDigital Hangfire Service");
                x.SetServiceName("yorkshiredigital-hangfire");
            });
        }

        private class Application
        {
            private IDisposable _host;

            public void Start()
            {
                _host = WebApp.Start<Startup>(Endpoint);

                Console.WriteLine();
                Console.WriteLine("Hangfire Server started.");
                Console.WriteLine("Dashboard is available at {0}/hangfire", Endpoint);
                Console.WriteLine();
            }

            public void Stop()
            {
                _host.Dispose();
            }
        }
    }
}
