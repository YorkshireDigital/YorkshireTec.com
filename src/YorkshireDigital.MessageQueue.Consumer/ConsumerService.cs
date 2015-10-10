using EasyNetQ;
using System;
using YorkshireDigital.MessageQueue.Messages;

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
            bus.SubscribeAsync<IHandleMessage>("textmessage_handler", msg =>
            {
                msg.Handle();
                return null;
            });
        }

        public void Stop()
        {
            // any shutdown code needed
        }
    }
}
