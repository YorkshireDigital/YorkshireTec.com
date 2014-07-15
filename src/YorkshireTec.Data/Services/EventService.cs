namespace YorkshireTec.Data.Services
{
    using global::NHibernate;
    using YorkshireTec.Data.Domain.Events;

    public class EventService
    {
        private readonly ISession session;

        public EventService(ISession session)
        {
            this.session = session;
        }

        public void Save(Event myEvent)
        {
            session.SaveOrUpdate(myEvent);
        }
    }
}
