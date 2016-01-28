using EasyNetQ;
using System;
using System.Configuration;
using NHibernate;
using YorkshireDigital.Data.Messages;
using YorkshireDigital.Data.NHibernate;
using YorkshireDigital.Data.Services;

namespace YorkshireDigital.MessageQueue.Consumer
{
    public interface IConsumerService
    {
        void Start();
        void Stop();
    }

    public class ConsumerService : IConsumerService
    {
        private readonly IBus bus;
        private ISessionFactory sessionFactory;
        private ISession session;
        public ConsumerService(IBus bus)
        {
            this.bus = bus;
        }

        public void Start()
        {
            var meetupService = bus.Advanced.Container.Resolve<IMeetupService>();
            
            sessionFactory = NHibernateSessionFactoryProvider.BuildSessionFactory(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);
            session = sessionFactory.OpenSession();

            bus.Subscribe<IHandleMeetupRequest>("IHandleMessage_subscription", msg =>
            {
                Console.WriteLine("IHandleMessage Found of type " + msg.GetType());
                msg.Handle(session, meetupService);
            });
        }

        public void Stop()
        {
            ((WindsorContainerWrapper)bus.Advanced.Container).Dispose();
            bus.Dispose();
            session.Dispose();
            sessionFactory.Dispose();
        }
    }
}
