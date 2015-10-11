using EasyNetQ;
using YorkshireDigital.Data.Messages;

namespace YorkshireDigital.Data.Tasks
{
    public class EventSyncTask
    {
        public void Execute(string eventId)
        {
            var message = new EventSyncMessage(eventId);

            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                bus.Publish<IHandleMessage>(message);
            }
        }
    }
}
