using EasyNetQ;
using System.Configuration;
using Castle.Windsor;
using System;
using Castle.MicroKernel.Registration;
using YorkshireDigital.MeetupApi.Clients;
using YorkshireDigital.Data.Services;

namespace YorkshireDigital.MessageQueue.Consumer
{
    public class BusBuilder
    {
        internal static IBus CreateMessageBus()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MessageQueue"];
            if (connectionString == null || connectionString.ConnectionString == string.Empty)
            {
                throw new Exception("easynetq connection string is missing or empty");
            }

            var container = new WindsorContainer();

            container.Register(
                Component.For<IMeetupClient>().ImplementedBy<MeetupClient>().LifestyleSingleton(),
                Component.For<IHangfireService>().ImplementedBy<HangfireService>().LifestyleSingleton(),
                Component.For<IMeetupService>().ImplementedBy<MeetupService>().LifestyleSingleton());

            RabbitHutch.SetContainerFactory(() =>
            {
                return new WindsorContainerWrapper(container);
            });

            return RabbitHutch.CreateBus(connectionString.ConnectionString);
        }
    }
}