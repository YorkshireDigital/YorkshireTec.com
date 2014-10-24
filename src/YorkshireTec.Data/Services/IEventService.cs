namespace YorkshireTec.Data.Services
{
    using System;
    using System.Collections.Generic;
    using YorkshireTec.Data.Domain.Events;

    public interface IEventService
    {
        void Save(Event myEvent);
        Event Get(string uniqueName);
        void Delete(Event eventToDelete);
        List<Event> GetWithinRange(DateTime from, DateTime to);
        List<Event> Query(DateTime? from, DateTime? to, string[] interests, string[] locations, int? skip, int? take);
    }
}