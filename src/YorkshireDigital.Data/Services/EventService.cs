namespace YorkshireDigital.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::NHibernate;
    using global::NHibernate.Linq;
    using YorkshireDigital.Data.Domain.Events;
    using YorkshireDigital.Data.Exceptions;

    public interface IEventService
    {
        void Save(Event myEvent);
        Event Get(string uniqueName);
        void Delete(string eventId);
        List<Event> GetWithinRange(DateTime from, DateTime to);
        List<Event> Query(DateTime? from, DateTime? to, string[] interests, string[] locations, int? skip, int? take, bool includeDeleted = false);
    }

    public class EventService : IEventService
    {
        private readonly ISession session;

        public EventService(ISession session)
        {
            this.session = session;
        }

        public void Save(Event eventToSave)
        {
            eventToSave.LastEditedOn = DateTime.UtcNow;
            session.SaveOrUpdate(eventToSave);
        }

        public Event Get(string uniqueName)
        {
            return session.Query<Event>()
                .Where(x => x.UniqueName == uniqueName)
                .Fetch(x => x.Group)
                .SingleOrDefault();
        }

        public void Delete(string eventId)
        {
            var eventToDelete = session.Get<Event>(eventId);
            if (eventToDelete == null)
                throw new EventNotFoundException(string.Format("No event found with unique name {0}", eventId));

            eventToDelete.DeletedOn = DateTime.UtcNow;
            session.SaveOrUpdate(eventToDelete);
        }

        public List<Event> GetWithinRange(DateTime from, DateTime to)
        {
            return session.Query<Event>().Where(x => x.Start >= from && x.Start <= to).ToList();
        }

        public List<Event> Query(DateTime? from, DateTime? to, string[] interests, string[] locations, int? skip, int? take, bool includeDeleted = false)
        {
            var query = session.Query<Event>();

            if (!includeDeleted)
            {
                query = query.Where(x => x.DeletedOn == null);
                query = query.Where(x => x.Group == null || x.Group.DeletedOn == null);
            }
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
