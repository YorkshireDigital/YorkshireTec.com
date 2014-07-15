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

        public Event Get(int id)
        {
            return session.Get<Event>(id);
        }

        public void Delete(Event eventToDelete)
        {
            session.Delete(eventToDelete);
        }
    }
}
