using Topshelf;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Hangfire;

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
                x.Service<IConsumerService>(s =>
                {
                    GlobalConfiguration.Configuration.UseSqlServerStorage("Database.Hangfire");
                    
                    s.ConstructUsing(name => container.Resolve<IConsumerService>());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc =>
                    {
                        tc.Stop();
                        // with Windsor you must _always_ release any components that you resolve.
                        container.Release(tc);
                        // make sure the container is disposed
                        container.Dispose();
                    });
                });

                x.RunAsLocalSystem();

                x.SetDescription("YorkshireDigital MQ Consumer Service");
                x.SetDisplayName("YorkshireDigital MQ Consumer Service");
                x.SetServiceName("yorkshiredigital-mq");
            });
        }
    }
}
