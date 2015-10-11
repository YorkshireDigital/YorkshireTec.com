namespace YorkshireDigital.Data.Tasks
{
    using EasyNetQ;
    using Messages;

    public class GroupSyncTask
    {
        public void Execute(string groupId)
        {
            var message = new GroupSyncMessage(groupId);

            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                bus.Publish<IHandleMessage>(message);
            }
        }
    }
}
