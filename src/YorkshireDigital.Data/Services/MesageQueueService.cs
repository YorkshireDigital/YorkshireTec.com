using EasyNetQ;
using YorkshireDigital.Data.Messages;

namespace YorkshireDigital.Data.Services
{
    public class MesageQueueService
    {
        public void AddEventSyncMessage(string eventId)
        {
            var message = new EventSyncMessage(eventId);

            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                bus.Publish<IHandleMeetupRequest>(message);
            }
        }
    }
}
