using System;
using Topshelf;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Hangfire;
using Serilog;

namespace YorkshireDigital.MessageQueue.Consumer
{
    public class Program
    {
        static void Main()
        {
            // create the container and run any installers in this assembly
            var container = new WindsorContainer().Install(FromAssembly.This());
            // start of the TopShelf configuration
            HostFactory.Run(x =>
            {
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.RollingFile(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\msmq-consumer-{Date}.log")
                    .WriteTo.ColoredConsole()
                    .MinimumLevel.Information()
                    .CreateLogger();

                x.Service<IConsumerService>(s =>
                {
                    GlobalConfiguration.Configuration.UseSqlServerStorage("Database.Hangfire");
                    
                    s.ConstructUsing(name => container.Resolve<IConsumerService>());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc =>
                    {
                        tc.Stop();
                        container.Release(tc);
                        container.Dispose();
                    });
                });

                x.RunAsLocalSystem();
                x.UseSerilog();

                x.SetDescription("YorkshireDigital MQ Consumer Service");
                x.SetDisplayName("YorkshireDigital MQ Consumer Service");
                x.SetServiceName("yorkshiredigital-mq");
            });
        }
    }
}
