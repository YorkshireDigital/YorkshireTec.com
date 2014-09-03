namespace YorkshireTec.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::NHibernate;
    using global::NHibernate.Linq;
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

        public List<Event> GetWithinRange(DateTime from, DateTime to)
        {
            return session.Query<Event>().Where(x => x.Start >= from && x.Start <= to).ToList();
        }

        public List<Event> Query(DateTime? from, DateTime? to, string[] interests, string[] locations, int? skip, int? take)
        {
            var query = session.Query<Event>();

            if (from.HasValue)
            {
                query = query.Where(x => x.Start >= from.Value);
            }
            if (to.HasValue)
            {
                query = query.Where(x => x.Start <= to.Value);
            }
            if (interests.Any())
            {
                query = query.Where(x => x.Interests.Any(i => interests.Contains(i.Name)));
            }


            return query.ToList();
        }
    }
}
