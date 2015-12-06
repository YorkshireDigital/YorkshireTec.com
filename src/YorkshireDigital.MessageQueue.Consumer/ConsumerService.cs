using EasyNetQ;
using System;
using YorkshireDigital.Data.Messages;
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
        public ConsumerService(IBus bus)
        {
            this.bus = bus;
        }

        public void Start()
        {
            var meetupService = bus.Advanced.Container.Resolve<IMeetupService>();

            bus.Subscribe<IHandleMeetupRequest>("IHandleMessage_subscription", msg =>
            {
                Console.WriteLine("IHandleMessage Found of type " + msg.GetType());
                msg.Handle(meetupService);
                msg.Dispose();
            });
        }

        public void Stop()
        {
            ((WindsorContainerWrapper)bus.Advanced.Container).Dispose();
            bus.Dispose();
        }
    }
}
