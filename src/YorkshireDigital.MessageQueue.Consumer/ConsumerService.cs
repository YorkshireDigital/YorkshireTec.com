using EasyNetQ;
using System;
using YorkshireDigital.Data.Messages;

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
            bus.Subscribe<IHandleMessage>("IHandleMessage_subscription", msg =>
            {
                Console.WriteLine("IHandleMessage Found of type " + msg.GetType());
                msg.Handle();
            });
        }

        public void Stop()
        {
            // any shutdown code needed
        }
    }
}
