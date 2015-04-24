namespace YorkshireDigital.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::NHibernate;
    using global::NHibernate.Linq;
    using YorkshireDigital.Data.Domain.Events;

    public class EventService : IEventService
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

        public Event Get(string uniqueName)
        {
            return session.Query<Event>()
                .Where(x => x.UniqueName == uniqueName)
                .Fetch(x => x.Organisation)
                .SingleOrDefault();
        }

        public void Delete(Event eventToDelete)
        {
            session.Delete(eventToDelete);
        }

        public List<Event> GetWithinRange(DateTime from, DateTime to)
        {
            return LinqExtensionMethods.Query<Event>(session).Where(x => x.Start >= from && x.Start <= to).ToList();
        }

        public List<Event> Query(DateTime? from, DateTime? to, string[] interests, string[] locations, int? skip, int? take)
        {
            var query = LinqExtensionMethods.Query<Event>(session);

            if (from.HasValue)
            {
                query = query.Where(x => x.Start >= from.Value);
            }
            if (to.HasValue)
            {
                query = query.Where(x => x.Start <= to.Value);
            }
            if (interests != null && interests.Any())
            {
                query = query.Where(x => x.Interests.Any(i => interests.Contains(i.Name)));
            }
            if (locations != null && locations.Any())
            {
                query = query.Where(x => locations.Contains(x.Location));
            }
            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }
            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.ToList();
        }
    }
}
