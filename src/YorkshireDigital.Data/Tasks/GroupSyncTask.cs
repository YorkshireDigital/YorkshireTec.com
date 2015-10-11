using EasyNetQ;
using YorkshireDigital.Data.Messages;

namespace YorkshireDigital.Data.Tasks
{
    public class GroupSyncTask
    {
        public void Execute(string groupId)
        {
            System.Console.WriteLine("Processing GroupSyncTask for GroupId " + groupId);

            var message = new GroupSyncMessage(groupId);

            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                bus.Publish<IHandleMeetupRequest>(message);
            }
            System.Console.WriteLine("Processing Complete");
        }
    }
}
