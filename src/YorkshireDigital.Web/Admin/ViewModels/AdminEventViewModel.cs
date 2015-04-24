namespace YorkshireDigital.Web.Admin.ViewModels
{
    using System;
    using AutoMapper;
    using YorkshireDigital.Data.Domain.Events;

    public class AdminEventViewModel
    {
        public string UniqueName { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Location { get; set; }
        public string Region { get; set; }
        public decimal Price { get; set; }
        public byte[] Photo { get; set; }
        public string GroupName { get; set; }
        public string GroupId { get; set; }

        public static AdminEventViewModel FromDomain(Event @event)
        {
            return Mapper.DynamicMap<Event, AdminEventViewModel>(@event);
        }

        public Event ToDomain()
        {
            return Mapper.DynamicMap<AdminEventViewModel, Event>(this);
        }

        public void UpdateDomain(Event @event)
        {
            Mapper.DynamicMap(this, @event);
        }
    }
}